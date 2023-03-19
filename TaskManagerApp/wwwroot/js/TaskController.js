// HERE WE WILL WRITE THE CODE TO FETCH THE TASK ACCORDING TO LIST SELECTED

// get all the list-menu-item elements
var listMenuItems = document.querySelectorAll('.list-menu-item');

// attach a click event listener to each list-menu-item element
listMenuItems.forEach(function (listMenuItem) {
    listMenuItem.addEventListener('click', function () {
        // remove the "active" class from all the list-menu-item elements
        listMenuItems.forEach(function (item) {
            item.classList.remove('is-active');
        });

        // add the "active" class to the clicked element
        this.classList.add('is-active');
        var id = this.name;

        // Adding list id into local session
        if (sessionStorage.length == 0) {
            // save list id into session
            // Save data to sessionStorage
            sessionStorage.setItem("ListID", id);
        }
        else {
            sessionStorage.removeItem("ListID")

            // Save data to sessionStorage
            sessionStorage.setItem("ListID", id);

        }

        getList(id);
    });
});

function getList(d) {
    //console.log(d);

    // create a new XMLHttpRequest object
    var xhr = new XMLHttpRequest();

    // handle the response
    xhr.onload = function () {
        if (xhr.status === 200) {
            // parse the response JSON
            var data = JSON.parse(xhr.responseText);
            //console.log(data.data);

            document.getElementById("task-item").innerHTML = "";

            if (data.data.length > 0) {
                for (var key in data.data) {
                    //console.log(data.data[key])

                    // check status of task
                    if (data.data[key].status) {
                        var temp = `
                                <div class="task-delete">
                                        <div class="task complete" name="${data.data[key].id}" onclick="toggleComplete(this)">
                                                <p>${data.data[key].task}</p>
                                        </div>
                                        <a href="/delete-task/${data.data[key].id}">
                                            <i class="fa-sharp fa-solid fa-trash"></i>
                                        </a>
                                </div>
                            `;
                        document.getElementById("task-item").innerHTML += temp;
                    }
                    else {
                        var temp = `
                                <div class="task-delete">
                                        <div class="task" name="${data.data[key].id}" onclick="toggleComplete(this)">
                                                <p>${data.data[key].task}</p>
                                        </div>
                                        <a href="/delete-task/${data.data[key].id}">
                                            <i class="fa-sharp fa-solid fa-trash"></i>
                                        </a>
                                </div>
                            `;
                        document.getElementById("task-item").innerHTML += temp;
                    }
                }
            }
            else {
                let temp = `
                                <div class="task">
                                    <p> No task assign yet !  </p>
                                </div>
                    `;
                document.getElementById("task-item").innerHTML += temp;
            }

        } else {
            console.log('Request failed. Returned status of ' + xhr.status);
        }
        //console.log("all well");
    };

    // configure the request
    xhr.open('GET', `http://localhost:5068/${d}/get-list`, true);

    // send the request
    xhr.send();
}