using Microsoft.IO;
using Newtonsoft.Json;
using System.Text;

namespace Gateway_ocelot_Solution.ApiLogs.PostgresLog
{
    public class PostgresApiLogsHandlerMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        private static readonly RecyclableMemoryStreamManager _streamManager = new();

        public async Task Invoke(HttpContext context, IPostgresApiLogsService logger)
        {
            var ignorePaths = new[] { "/", "/favicon.ico" };
            var requestPath = context.Request.Path.Value?.ToLowerInvariant();

            if (ignorePaths.Contains(requestPath))
            {
                await _next(context);
                return;
            }

            var contentType = context.Response.ContentType?.ToLowerInvariant();
            if (contentType?.StartsWith("application/octet-stream") == true ||
                contentType?.StartsWith("application/pdf") == true ||
                contentType?.StartsWith("application/zip") == true ||
                contentType?.StartsWith("text/csv") == true ||
                contentType?.StartsWith("application/vnd.ms-excel") == true ||
                contentType?.StartsWith("image/") == true)
            {
                await _next(context);
                return;
            }

            context.Request.EnableBuffering();
            var originalBodyStream = context.Response.Body;

            var responseBody = _streamManager.GetStream(); // ✅ ใช้ stream จาก pool

            try
            {
                context.Response.Body = responseBody;

                var requestBody = await ReadRequestBodyAsync(context.Request);

                await _next(context);

                string responseBodyText = await ReadResponseBodySafeAsync(context.Response);

                var allowedMethods = new[] { "GET", "POST", "PUT", "DELETE" };

                if (allowedMethods.Contains(context.Request.Method.ToUpperInvariant()))
                {
                    var requestBodyText = JsonConvert.SerializeObject(requestBody);
                    var requestSizeInBytes = Encoding.UTF8.GetByteCount(requestBodyText ?? "");
                    //  var responseSizeInBytes = Encoding.UTF8.GetByteCount(responseBodyText ?? "");
                    var requestSizeInKb = requestSizeInBytes / 1024.0;
                    //var responseSizeInKb = responseSizeInBytes / 1024.0;

                    context.Response.Headers.TryGetValue("Content-Length", out var contentLengthValue);

                    long contentLength = 0;
                    if (!string.IsNullOrWhiteSpace(contentLengthValue))
                    {
                        long.TryParse(contentLengthValue.ToString(), out contentLength);

                    }
                    double contentLengthKb = contentLength / 1024.0;

                    logger.Log(new LogEntry
                    {
                        Timestamp = DateTime.UtcNow,
                        Level = "Information",
                        Message = "Request/Response logged",
                        Method = context.Request.Method,
                        Path = context.Request.Path,
                        StatusCode = context.Response.StatusCode,
                        Request = requestBody,
                        Response = TryParseJson(responseBodyText),
                        RequestSizeKb = Math.Round(requestSizeInKb, 2),
                        ResponseSizeKb = Math.Round(contentLengthKb, 2)
                    });
                }

                context.Response.Body = originalBodyStream;

                if (responseBody.CanSeek)
                    responseBody.Seek(0, SeekOrigin.Begin);

                await responseBody.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                logger.Log(new LogEntry
                {
                    Timestamp = DateTime.UtcNow,
                    Level = "Error",
                    Message = "Unhandled exception in middleware",
                    Method = context.Request.Method,
                    Path = context.Request.Path,
                    Exception = ex.ToString()
                });
                //Console.WriteLine("Middleware Error: " + ex.Message);
                //Console.WriteLine(ex.StackTrace);
                throw;
            }
            finally
            {
                await responseBody.DisposeAsync(); // ✅ คืน memory ให้ pool
            }
        }

        private async Task<object> ReadRequestBodyAsync(HttpRequest request)
        {
            if (request.ContentType?.StartsWith("multipart/form-data", StringComparison.OrdinalIgnoreCase) == true)
            {
                if (!request.HasFormContentType) return "[Invalid multipart content]";

                try
                {
                    var form = await request.ReadFormAsync();
                    var filesInfo = form.Files.Select(f => new
                    {
                        f.Name,
                        f.FileName,
                        f.ContentType,
                        f.Length
                    });

                    var fields = form
                        .Where(kv => kv.Value.Count > 0)
                        .ToDictionary(kv => kv.Key, kv => kv.Value.ToString());

                    return new { Files = filesInfo, Fields = fields };
                }
                catch
                {
                    return "[Failed to parse multipart form]";
                }
            }

            request.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Seek(0, SeekOrigin.Begin);
            return TryParseJson(body);
        }

        private async Task<string> ReadResponseBodySafeAsync(HttpResponse response)
        {
            if (!response.Body.CanSeek)
                return "[Non-seekable response stream]";

            response.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(response.Body, Encoding.UTF8, leaveOpen: true);
            var text = await reader.ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }

        private object TryParseJson(string input)
        {
            try { return JsonConvert.DeserializeObject(input); }
            catch { return input; }
        }
    }
}
