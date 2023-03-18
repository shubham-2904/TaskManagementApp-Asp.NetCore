// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// function for adding new list into database
function addList() {
    console.log("Adding new list function");

    location.href = '/new-list';

}

// function to calling new task into database
function addTask() {
    console.log("hello world")

    location.href = `/${sessionStorage.getItem("ListID")}/`;
}

// windows on load
// If we have the session so we will render all task from database connected 
// to that list
window.onload = () => {
    if (sessionStorage.length != 0) {
        let listID = sessionStorage.getItem("ListID");
        
        console.log("hello world");

        for (let i = 0; i < listMenuItems.length; i++) {

            listMenuItems[i].classList.remove('is-active');
            if (listID === listMenuItems[i].name) {
                listMenuItems[i].classList.add("is-active")
            }
        }

        var id = sessionStorage.getItem("ListID");
        getList(id);
    }
}


// Function to add complete status 
function toggleComplete(element) {
    var taskId = element.attributes.name.nodeValue;
    SendTaskComplete(element, taskId);
}

function SendTaskComplete(element, taskId) {
    // create a new XMLHttpRequest object
    var xhr = new XMLHttpRequest();

    // handle the response
    xhr.onload = function () {
        if (xhr.status === 200) {
            // parse the response JSON
            var data = JSON.parse(xhr.responseText);
            //console.log(data.data);
            //console.log(data.data.status)

            if (!data.data.status) {
                element.classList.remove("complete");
            }
            else {
                element.classList.add("complete");
            }
            

        } else {
            console.log('Request failed. Returned status of ' + xhr.status);
        }
        //console.log("all well");
    };

    // configure the request
    xhr.open('GET', `http://localhost:5068/task-complete/${ taskId }`, true);

    // send the request
    xhr.send();
}

// Removie localStorage Session
function logout() {
    sessionStorage.removeItem("ListID")
}

