var formatDateTimePicker = "dd/MM/yyyy HH:mm";
var formatDatePicker = "dd/MM/yyyy";
var formatDateWithTime = "dd/MM/yyyy HH:mm:ss";
var formatDateWithTime_export = "yyyyMMddHHmmss";
var formatTime = "HH:mm";

/** Call Api Start **/

//function APIPost(url, data) {
//    var result;


//  axios.post(url, data)
//        .then(function (response) {
//            console.log(response.data);
//           result= response.data
//        }).catch(function (error) {
//            console.log(error);
//        });
//    return result;
//}
function getAccessToken() {
    const name = "uacj-tms-access-token=";
    const decodedCookie = decodeURIComponent(document.cookie);
    const cookies = decodedCookie.split(";");

    for (let i = 0; i < cookies.length; i++) {
        let c = cookies[i].trim();
        if (c.startsWith(name)) {
            return c.substring(name.length);
        }
    }
    return null;
}
function getAccessRefreshToken() {
    const name = "uacj-tms-refresh-token=";
    const decodedCookie = decodeURIComponent(document.cookie);
    const cookies = decodedCookie.split(";");

    for (let i = 0; i < cookies.length; i++) {
        let c = cookies[i].trim();
        if (c.startsWith(name)) {
            return c.substring(name.length);
        }
    }
    return null;
}
axios.interceptors.request.use((config) => {
    const token = getAccessToken(); // ดึงจาก memory, storage, หรือ cookie
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

async function navbar_setLanguage(lang) {
    const response = await APIPost("/refresh/token", {
        RefreshToken: getAccessRefreshToken(),
        Token: getAccessToken(),
        LanguageCode: lang
    });

    if(response.status == "OK"){
        location.reload();
    }
}
function autoRefreshToken(token_timeout) {
    const refreshInterval = (token_timeout - 5) * 60 * 1000; 

    async function refresh() {
        const refreshToken = getAccessRefreshToken();
        if (refreshToken == null) {
            return;
        }

        try {
            const response = await APIPost("/refresh/token", {
                RefreshToken: getAccessRefreshToken(),
                LanguageCode: CurrentCulture_now
            });

           // if (!response.ok) throw new Error("Refresh failed");

        
            console.log("Token refreshed");
            if (response.status != "OK") {
               window.location.href = "/Account/Logout";
            }
     

            // 🔁 ตั้งตัวเองใหม่
            setTimeout(refresh, refreshInterval);
        } catch (error) {
            console.log("error =>", error);
           // window.location.href = "/Account/Logout"; // or show popup
        }
    }

    // เริ่ม process
    setTimeout(refresh, refreshInterval);
}


function convertLocalTimeToUTC(data) {
    const dateTimeRegex = /^\d{4}-\d{2}-\d{2}[T\s]\d{2}:\d{2}:\d{2}(\.\d+)?([+-]\d{2}:\d{2})?$/;// Match ISO datetime

    if (Array.isArray(data)) {
        return data.map(item => convertLocalTimeToUTC(item));
    } else if (typeof data === 'object' && data !== null) {
        Object.keys(data).forEach(key => {
            if (typeof data[key] === 'string' && dateTimeRegex.test(data[key])) {
                let localDate = new Date(data[key]); // Create date from local time string
                let utcDate = new Date(localDate.getTime() - localDate.getTimezoneOffset() * 60000); // Adjust to UTC
                data[key] = utcDate.toISOString(); // Convert to ISO string in UTC
            } else if (typeof data[key] === 'object' && data[key] !== null) {
                data[key] = convertLocalTimeToUTC(data[key]); // Recursively process inner objects
            }
        });
    }
    return data;
}
const dateTransformer = function (data) {
    const dateTimeRegex = /^\d{4}-\d{2}-\d{2}[T\s]\d{2}:\d{2}:\d{2}(\.\d+)?([+-]\d{2}:\d{2})?$/;
    if (data instanceof Date || (typeof data === 'string' && dateTimeRegex.test(data) )) {
        

        //data = convertLocalTimeToUTC(data);
        //return moment(data).utc().format("MM/DD/YYYY HH:mm:ss.SSSSSS");

        return moment(data).format("MM/DD/YYYY HH:mm:ss.SSSSSS");
        //return "10/15/2023 02:57:23"
    } else if (data === "") {
        return null
    }


    if (Array.isArray(data)) {
        return data.map(dateTransformer);
    }
    if (typeof data === 'object' && data !== null) {
        return Object.fromEntries(Object.entries(data).map(([key, value]) => [key, dateTransformer(value)]));
    }
    return data;
};



let call_Apitimeout = 3600 * 1000

function convertDatesToLocalTime(data) {

    return data;
    function getTimeZoneOffsetHours() {
        return -new Date().getTimezoneOffset() / 60; // แปลงค่า offset จากนาทีเป็นชั่วโมง
    }

    const offsetHours = getTimeZoneOffsetHours(); // ดึง offset ชั่วโมงของเครื่อง
    const dateTimeRegex = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(\.\d+)?$/; // ตรวจสอบรูปแบบ datetime

    if (Array.isArray(data)) {
        return data.map(item => convertDatesToLocalTime(item));
    } else if (typeof data === 'object' && data !== null) {
        Object.keys(data).forEach(key => {
            if (typeof data[key] === 'string' && dateTimeRegex.test(data[key])) {
                let date = new Date(data[key]);
                date.setHours(date.getHours() + offsetHours); // ปรับเวลาให้เป็นโซนของเครื่อง
                data[key] = date; // แปลงเป็นเวลาท้องถิ่นแบบอ่านง่าย
            } else if (typeof data[key] === 'object' && data[key] !== null) {
                data[key] = convertDatesToLocalTime(data[key]); // ทำซ้ำกับ object ข้างใน
            }
        });
    }
    return data;
}

let TextAPI_Error = "Could not connect the internet. Please contact IT support.";

async function APIGet(url, data) {
    try {

        //console.log(JSON.stringify(body));
        const response = await axios.get(url, dateTransformer(data), {
            timeout: call_Apitimeout, maxRedirects: 0,
            validateStatus: (status) => {
                return status >= 200 && status < 400; // Default validation status (2xx and 3xx)
            }
            //, validateStatus: () => true
        });
        if (response.headers['content-type'] === 'text/html; charset=utf-8') {
            window.location.href = response.request.responseURL;
        }
        //return response.data; // Return the response data if the request is successful
        const formattedData = convertDatesToLocalTime(response.data);

        return formattedData; // คืนค่าข้อมูลที่แปลงแล้ว
    } catch (error) {
        if (error.response && error.response.status === 401) {
            window.location.href = "/Account/Logout";
            return; // หยุด execution หลัง redirect
        }

        //Could not connect the internet. Please contact IT support.
        /*console.error("APIGet Error:", error.message);*/
        Event.showError(TextAPI_Error);
        throw TypeError(error.message);
    }
}


async function APIPost(url, data) {
    try {

        //console.log(JSON.stringify(body));
        const response = await axios.post(url, dateTransformer(data), {
            timeout: call_Apitimeout, maxRedirects: 0,
            validateStatus: (status) => {
                return status >= 200 && status < 400; // Default validation status (2xx and 3xx)
            }
         
        });
        if (response.headers['content-type'] === 'text/html; charset=utf-8') {
            window.location.href = response.request.responseURL;
        }
        // แปลงค่าทุก field ที่เป็น DateTime ใน response.data ให้เป็นเวลาท้องถิ่น
        const formattedData = convertDatesToLocalTime(response.data);

        return formattedData; // คืนค่าข้อมูลที่แปลงแล้ว
    } catch (error) {
        if (error.response && error.response.status === 401) {
            window.location.href = "/Account/Logout";
            return; // หยุด execution หลัง redirect
        }
        /*console.error("APIGet Error:", error.message);*/
        Event.showError(TextAPI_Error);
        throw TypeError(error.message);
    }
}

async function APIPostDownload(url, data) {
    try {

        //console.log(JSON.stringify(body));
        const response = await axios.post(url, dateTransformer(data), {
            timeout: call_Apitimeout, maxRedirects: 0,
            responseType: 'blob',
            validateStatus: (status) => {
                return status >= 200 && status < 400; // Default validation status (2xx and 3xx)
            }

        });
        if (response.headers['content-type'] === 'text/html; charset=utf-8') {
            window.location.href = response.request.responseURL;
        }
        return response; // Return the response data if the request is successful
    } catch (error) {
        if (error.response && error.response.status === 401) {
            window.location.href = "/Account/Logout";
            return; // หยุด execution หลัง redirect
        }
        /*console.error("APIGet Error:", error.message);*/
        Event.showError(TextAPI_Error);
        throw TypeError(error.message);
    }
}

function objectToFormData(obj) {
    const formData = new FormData();

    Object.entries(obj).forEach(([key, value]) => {

        if (value instanceof Date) {
            formData.append(key, moment(value).format("MM/DD/YYYY HH:mm:ss"));
           
        } else if (value === "" || value == null) {
            //return 
           // formData.append(key, null);
        }
        else {
            formData.append(key, value);
        }

    });

    return formData;
}
async function APIPostUploadFile(url, data) {
    try {

        //console.log(objectToFormData(dateTransformer(data)));
        const response = await axios.post(url, objectToFormData(data), {
            timeout: call_Apitimeout,
            maxRedirects: 0,
            //validateStatus: () => true,
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        });
        if (response.headers['content-type'] === 'text/html; charset=utf-8') {
            window.location.href = response.request.responseURL;
        }
        return response.data; // Return the response data if the request is successful
    } catch (error) {

        $.notify({
            icon: 'fas fa-times-circle',
            message: error.response
        }, {
            delay: 3000,
            type: 'danger'
        });


        throw TypeError(error.message);
    }
}

async function APIPostUploadFileRaw(url, data) {
    try {

        const formData = new FormData();
        formData.append('file', data);
        const response = await axios.post(url, formData, {
            timeout: call_Apitimeout,
            maxRedirects: 0,
            //validateStatus: () => true,
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        });
        if (response.headers['content-type'] === 'text/html; charset=utf-8') {
            window.location.href = response.request.responseURL;
        }
        return response.data; // Return the response data if the request is successful
    } catch (error) {

        $.notify({
            icon: 'fas fa-times-circle',
            message: error.response
        }, {
            delay: 3000,
            type: 'danger'
        });


        throw TypeError(error.message);
    }
}



/** Call Api End  **/


/*** Function Fix bug kendo Start ***/

function SetFormatsNumber(Number) {
    try {
        return Number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    } catch (ex) {
        return "";
    }
}

function setFileName(NameFile) {
    var date = new Date();

    return NameFile + "_" + (date.getFullYear() + ("0" + (date.getMonth() + 1)).slice(-2) + ("0" + date.getDate()).slice(-2) + "_" + ("0" + date.getHours()).slice(-2) + ("0" + date.getMinutes()).slice(-2) + ("0" + date.getSeconds()).slice(-2));
}


function convertToHHMM(timeString) {
    // Split the time string into an array of [hours, minutes, seconds]
    const parts = timeString.split(':');

    // Extract the hours and minutes
    const hours = parts[0];
    const minutes = parts[1];

    // Format and return the string as 'hh:mm'
    return `${hours}:${minutes}`;
}
function diffInMinutes(startTime, endTime) {

    startTime = startTime.substring(0, 5);
    endTime = endTime.substring(0, 5);

    function timeToMinutes(time) {
        const [hours, minutes] = time.split(':').map(Number);
        return hours * 60 + minutes;
    }

    const startMinutes = timeToMinutes(startTime);
    const endMinutes = timeToMinutes(endTime);

    // ตรวจสอบว่าเวลาเดียวกันหรือไม่
    if (startMinutes === endMinutes) {
        return 1440; // 24 ชั่วโมง = 1440 นาที
    }

    // ตรวจสอบว่าต้องข้ามวันหรือไม่
    let diffMinutes;
    if (endMinutes >= startMinutes) {
        diffMinutes = endMinutes - startMinutes;
    } else {
        diffMinutes = (24 * 60 - startMinutes) + endMinutes;
    }

    return diffMinutes;
}

function setNameExtension(extension) {

    var extension = "." + extension.split('.').pop();
    return extension
}

function SetFormatsDate(date) {

    let result = null
    try {
        if (date != null) {
            result = kendo.toString(kendo.parseDate(date, 'yyyy-MM-ddTHH:mm:ss'), formatDateWithTime)

            if (result == null) {
                result = kendo.toString(date, formatDateWithTime)
            }

        } else {
            result = ""
        }

    } catch (e) {

        result = ""
    }

    return result

}


var AutoCompleteChangeIsValid = false;
// This was a valid account! Set the accountIsValid flag.
function AutoCompleteSelect(e) {
    AutoCompleteChangeIsValid = true;
}
// Clear the account field if the choice was not valid
function AutoCompleteChange(e) {
    if (!AutoCompleteChangeIsValid) {
        this.value('');
    }
    // Reset the accountIsValid flag for next run
    AutoCompleteChangeIsValid = false;
}


function groupBy(list, keyGetter) {
    const map = new Map();
    list.forEach((item) => {
        const key = keyGetter(item);
        const collection = map.get(key);
        if (!collection) {
            map.set(key, [item]);
        } else {
            collection.push(item);
        }
    });
    return map;
}



/*** Function Fix bug kendo End ***/



function GenerateId() {
    var result = '';
    var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    var charactersLength = characters.length;
    for (var i = 0; i < 5; i++) {
        result += characters.charAt(Math.floor(Math.random() *
            charactersLength));
    }
    return result;
}


function format(str, col) {
    col = typeof col === 'object' ? col : Array.prototype.slice.call(arguments, 1);

    return str.replace(/\{\{|\}\}|\{(\w+)\}/g, function (m, n) {
        if (m == "{{") { return "{"; }
        if (m == "}}") { return "}"; }

        if (col[n] == undefined) {
            return "";
        }

        return col[n] !== undefined ? col[n] : `{${n}}`;
    });
};



//showAlert

function showError(message) {
    app.ui.showAlertWarning("#message-container", message);
}

function showSuccess(message) {
    $.notify({
        icon: 'fas fa-check-circle fs-lg',
        message: message
    }, {
        delay: 3000,
        type: 'success'
    });
    app.ui.showAlertSuccess("#message-container", message);
}

function showWarning(message) {
    app.ui.showAlertWarning("#message-container", message);
}
//showAlert



// encode decode
function encode(data) {
    try {
        if (data == null) return null;
        return btoa(data);
    }
    catch {
        return null;
    }
}

function decode(data) {
    try {

        if (data == null) return null;
        return atob(data)
    }
    catch {
        return null;
    }
}




function urlParams(Id) {
    try {
        const queryString = window.location.search;
        const urlParams = new URLSearchParams(queryString);
        const page_type = urlParams.get(Id);

        if (page_type != "") {
            return page_type;
        } else {
            return null;
        }
    }
    catch {
        return null;
    }
}
function toDateOnlyIso(date) {
    if (date instanceof Date && !isNaN(date)) {
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        const localDate = `${year}-${month}-${day}`;
        return localDate; // yyyy-MM-dd
    }
    return null;
}   

function onDatePicker(e) {
    //const date = new Date(this.value());
    const picker = e.sender;
    const date = picker.value();

    if (date) {
        const dateOnly = new Date(date.getFullYear(), date.getMonth(), date.getDate());
        picker.value(dateOnly); // ตัดเวลาออก
    }
}
function onDateTimePicker(e) {
    //const date = new Date(this.value());
    const date = kendo.parseDate(this.value(), "dd-MM-yy HH:mm");
    if (date == null) {
        this.value(null);
        return
    }
    //const dateTimeNow = new Date();;
    //if (date.getHours() == 0 && date.getMinutes() == 0) {
    //    date.setHours(dateTimeNow.getHours());
    //    date.setMinutes(dateTimeNow.getMinutes());
    //    this.value(date);
    //}
}



function saveCapture(element) {
    return html2canvas(element, {
        scale: 5,
        dpi: 300,
    }).then(function (canvas) {
        console.log("canvas =>", canvas, canvas.toDataURL("image/png"));
        return canvas.toDataURL("image/png");
    })
}


function onNotificationShow(e) {
    //document.querySelector('main').classList.add('disabled-input-screen-load')

    document.getElementById('site-section').classList.add('disabled-input-screen-load');
}

function onNotificationHide(e) {
    //document.querySelector('main').classList.remove('disabled-input-screen-load')

    document.getElementById('site-section').classList.remove('disabled-input-screen-load');
}
//site-section


function Resources(screenCode, objectId, col) {
    const screenData = _data_LocalizedResources[screenCode];
    col = typeof col === 'object' ? col : Array.prototype.slice.call(arguments, 2);
    if (screenData) {
        let result = null
        try {
            if (CurrentCulture_now == "th") {
                result = screenData.find(item => item.ObjectID === objectId).ResourcesTH;
            } else {
                result = screenData.find(item => item.ObjectID === objectId).ResourcesEN;

            }
            if (col != null) {
                result = format(result, col)
            }


        } catch (e) {
            console.log("Error", e);
            return screenCode + "_" + objectId;
        }
        return result || screenCode + "_" + objectId;
    }
    return screenCode + "_" + objectId;
}


function Message(MessageType, messagecode, col) {
    const screenData = _data_LocalizedMessages[MessageType];
    col = typeof col === 'object' ? col : Array.prototype.slice.call(arguments, 2);
    if (screenData) {
        let result = null
        try {
            if (CurrentCulture_now == "th") {
                result = screenData.find(item => item.MessageCode === messagecode).MessageNameTH;
            } else {
                result = screenData.find(item => item.MessageCode === messagecode).MessageNameEN;

            }
            if (col != null) {
                result = format(result, col)
            }


        } catch (e) {
            console.log("Error", e);
            return MessageType + "_" + messagecode;
        }


        return result || MessageType + "_" + messagecode;
    }
    return MessageType + "_" + messagecode;
}

let Event = {
    clearMessage: () => {
        app.ui.clearAlert("#message-container");
    },
    showWarning: (message) => {

        $.notify({
            icon: 'fas fa-exclamation-triangle fs-lg',
            message: message
        }, {
            delay: 3000,
            type: 'warning'
        });
        app.ui.showAlertWarning("#message-container", message);
    },

    showSuccess: (message) => {
        $.notify({
            icon: 'fas fa-check-circle fs-lg',
            message: message
        }, {
            delay: 3000,
            type: 'success'
        });
        app.ui.showAlertSuccess("#message-container", message);
    },
    showError: (message) => {
        $.notify({
            icon: 'fas fa-times-circle',
            message: message
        }, {
            delay: 3000,
            type: 'danger'
        });
        //app.ui.showAlertError("#message-container", message);
    }
}


async function isNullOrEmpty(value) {
    return value == null || value == "";
}

function convertToBoolean(value) {
    switch (value) {
        case '1':
            return true;
        case '0':
            return false;
        case '':
            return null;
        default:
            return null;  // Handle unexpected values
    }
}

function filterAll(e, control, field1, field2) {
    var filterValue = e.filter != undefined ? e.filter.value : "";
    e.preventDefault();
    //console.log(control)
    control.dataSource.filter({
        logic: "or",
        filters: [
            {
                field: field1,
                operator: "contains",
                value: filterValue
            },
            {
                field: field2,
                operator: "contains",
                value: filterValue
            }
        ]
    });
}

function formatNumber(num) {
    // Create a number formatter
    var formatter = new Intl.NumberFormat('en-US', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });

    // Format the number
    return formatter.format(num);
}
function convertToDDMMYYYY(dateStr) {
    const date = new Date(dateStr);

    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Months are zero-based
    const year = date.getFullYear();

    return `${day}/${month}/${year}`;
}


function createInput(input) {
    //console.log(
    //`$("#${input}").kendoTextBox({
    //    //enable: false
    //});
    //${input.replace(/-/g, "_")} = $("#${input}").data("kendoTextBox")`
    //);
    return `$("#${input}").kendoTextBox({
        
    });
    ${input.replace(/-/g, "_")} = $("#${input}").data("kendoTextBox");`
}


/**
CreateInputIds("qos200-window-dialog")
CreateParameterIds("qos200-window-dialog")
 */
function CreateInputIds(divId) {
    // Select the div by its ID
    const container = document.getElementById(divId);

    // Find all input elements within the selected div
    const inputs = container.querySelectorAll('input');


    let idsArray = ``;

    for (let i = 0; i < inputs.length; i++) {
        //idsArray.push(inputs[i].id);
        if (i != 0) {
            idsArray += `\n`;
        }
        idsArray += createInput(inputs[i].id);
    }

    console.log(idsArray);
    // Extract IDs into an array
    // return Array.from(inputs, input => input.id);
}
function CreateParameterIds(divId) {
    const container = document.getElementById(divId);

    // Find all input elements within the selected div
    const inputs = container.querySelectorAll('input');


    let idsArray = `let `;
    for (let i = 0; i < inputs.length; i++) {
        if (i % 6 === 0 && i != 0) {
            idsArray += `;`;
            idsArray += `\nlet `;
        }
        if (i % 6 != 0 && i != 0) {
            idsArray += `,`;
        }
        idsArray += (inputs[i].id).replace(/-/g, "_");

    }
    idsArray += `;`;
    console.log(idsArray);
}


function downloadFile(file, fileName) {
    const anchorElement = document.createElement('a');
    document.body.appendChild(anchorElement);
    anchorElement.style.display = 'none';
    const url = window.URL.createObjectURL(file);
    anchorElement.href = url;
    anchorElement.download = fileName;
    anchorElement.click();
    window.URL.revokeObjectURL(url);
}

function downloadTemplate(href, fileName) {
    const downloadLink = document.createElement('a');
    downloadLink.href = href;
    downloadLink.download = fileName;
    document.body.appendChild(downloadLink);
    downloadLink.click();
}

// add button date range
function addButtonDateRange(id) {
    $('#' + id).find('.k-dateinput').append('<button aria-label="select" tabindex="-1" onclick="buttonDateRangeClick()" class="k-input-button k-button k-icon-button k-button-md k-button-solid k-button-solid-base" type="button" role="button"><span class="k-icon k-i-calendar k-button-icon"></span></button>');
    $('#' + id).find('span').eq(4).append('<label class="mx-2">- </label>');
}

function buttonDateRangeClick() {
    $("#sc-delivery-date").find('input').trigger("click");
}


var TEMPLATE_PURCHASE_ORDER = /^IOS100\|(?<PurchaseOrderNo>.+)\|(?<VendorCode>.+)$/;
var TEMPLATE_RM_LABEL = /^IOS200\|(?<ControlNo>.+)\|(?<PartCode>.+)\|(?<PartName>.+)$/;
var TEMPLATE_FG_LABEL = /^IOS200\|(?<ControlNo>.+)\|(?<ModelCode>.+)\|(?<ModelName>.+)$/;

function checkFormatQRCode(qr, type) {
    if (type === 'IOS200') {
        return (TEMPLATE_RM_LABEL.exec(qr));

    } else if (type === 'IOS100') {
        return (TEMPLATE_PURCHASE_ORDER.exec(qr));
    }

    return false;
}


function getLocalTimezoneOffset() {
    return -new Date().getTimezoneOffset();
}

function checkExclusiveUpdate(serverDateString, localDateString) {
    const serverDate = new Date(serverDateString);
    const localDate = new Date(localDateString);

    // เปรียบเทียบโดยตัด millisecond ออก
    const stripMilliseconds = (date) =>
        new Date(
            date.getFullYear(),
            date.getMonth(),
            date.getDate(),
            date.getHours(),
            date.getMinutes(),
            date.getSeconds()
        );

    const serverTime = stripMilliseconds(serverDate).getTime();
    const localTime = stripMilliseconds(localDate).getTime();

    if (serverTime !== localTime) {
        messageDialog.error(Message("Warning", "DataAlreadyUpdated"), () => { });
        return false;
    }

    return true;
}
