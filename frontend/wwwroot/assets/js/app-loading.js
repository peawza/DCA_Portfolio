function formatDateTimeLocal(date) {
    const pad = n => n.toString().padStart(2, '0');
    const day = pad(date.getDate());
    const month = pad(date.getMonth() + 1);
    const year = date.getFullYear();
    const hour = pad(date.getHours());
    const minute = pad(date.getMinutes());
    return `${day}/${month}/${year} | ${hour}:${minute}`;
}

function updateDateTime() {
    const now = new Date();
    document.getElementById("user-info-datetime-span").textContent = formatDateTimeLocal(now);
}

// เรียกครั้งแรก
updateDateTime();

// ตั้ง interval ให้ทำทุก 1 วินาที
setInterval(updateDateTime, 1000);


let api_axios_delay = 200;
function onNotificationShow(e) {
    document.getElementById('loadingOverlay').classList.remove('d-none');
    document.getElementById('loadingOverlay').innerText = '';
}

function onNotificationHide(e) {
    document.getElementById('loadingOverlay').classList.add('d-none');
    document.getElementById('loadingOverlay').innerText = '';
}
var axios_loading = $("#axios-loading").kendoNotification({
    show: onNotificationShow, hide: onNotificationHide,
    position: {
        pinned: true,
        bottom: 10,
        left: 10
    },
    animation: {
        open: {
            effects: "fadeIn"
        },
        close: {
            effects: "fadeOut"
        }
    },
    width: 400,
    autoHideAfter: 0,
    stacking: "down",
    templates: [
        {
            type: "progress",
            template: "<div class=\"\"><i class=\"fa-duotone fa-spinner-third fa-spin\"></i> #= message #</div>"
        }
    ]
}).data("kendoNotification");
$(document).ready(async function () {
    try {
        if ($("#ajax-notifications").data("kendoNotification") != undefined) {
            $("#ajax-notifications").data("kendoNotification")._events = $("#axios-loading").data("kendoNotification")._events
        }
    } catch (e) {
        console.error("", e);
    }

    //const fullPath = window.location.pathname; 
    //const segments = fullPath.split('/').filter(s => s); 

   
    //if (segments.length > 1) {
    //    const partialPath = segments.slice(0, -1).join('/') + '/'; 
    //    document.getElementById('nav-pathpage').textContent = partialPath;

    //    const rootPath = segments[0]; 
    //    const navRoot = document.getElementById('nav-root-pathpage');
    //    if (navRoot) {
    //        navRoot.textContent = rootPath;
    //    }
    //}

    const userInfo = document.querySelector('.user-info');
    const dropdownMenu = document.querySelector('.navdropdown-menu');
    const dropdownIcon = document.querySelector('.dropdown-icon');

    userInfo.addEventListener('click', () => {
        dropdownMenu.classList.toggle('open');
        dropdownIcon.classList.toggle('open'); 
    });

    
    document.addEventListener('click', (event) => {
        if (!userInfo.contains(event.target) && !dropdownMenu.contains(event.target)) {
            dropdownMenu.classList.remove('open');
            dropdownIcon.classList.remove('open');
        }
    });


});


let api_axios_count = 0;
function delay(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}
axios.interceptors.request.use(function (config) {
    api_axios_count += 1;

    let axios_loading_length = document.querySelectorAll("." + axios_loading._guid).length;

    if (axios_loading_length == 0 && document.readyState === 'complete') {

        axios_loading.show({ message: 'Please wait while data is loading..' }, "progress");
    }



    return config;
}, async function (error) {
    api_axios_count--;
    setTimeout(() => {
        if (api_axios_count == 0 && document.readyState === 'complete') {
            axios_loading.hide();
        }
    }, api_axios_delay);


    return Promise.reject(error);
});



// Add a response interceptor
axios.interceptors.response.use(async function (response) {
    api_axios_count--;
    setTimeout(() => {
        if (api_axios_count == 0) {
            axios_loading.hide();
        }
    }, api_axios_delay);

    return response;
}, async function (error) {
    api_axios_count--;
    setTimeout(() => {
        if (api_axios_count == 0) {
            axios_loading.hide();
        }
    }, api_axios_delay);

    return Promise.reject(error);
});





window.addEventListener('load', async (event) => {
    if (document.readyState === 'complete') {

        await delay(500);
        //document.getElementById('loadingOverlay').classList.add('d-none');
        document.getElementById('page-content').classList.remove('d-hidden');
        //console.log('Document and all sub-resources have finished loading');
    }
    document.getElementById('loadingOverlay').classList.add('d-none');
});