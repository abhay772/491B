
async function loadCrimeMapPage(homepageContent, username, userrole) {
    // fetch crime map page html
    fetch("./Views/crimeMap.html")
        .then((response) => response.text())
        .then(async (data) => {
            homepageContent.innerHTML = data;

            if (userrole !== "Property Manager" && userrole !== "Service Provider") {
                loadHomePage(userrole, username);
            }

            const crimeMapContent = document.getElementById("crime-map-content");

            // add crime map 
            const imageContainer = await loadImageContainer(homepageContent, crimeMapContent, username, userrole);

            // add "Add Crime Alert" button
            const addCrimeAlertButton = document.getElementById("addAlertButton");
            addCrimeAlertButton.addEventListener("click", function () {
                if (userrole !== "Property Manager") {
                    loadCrimeMapPage(homepageContent, username, userrole);
                }

                // create a popup text notification to click the location on the map
                const notification = document.createElement("div");
                notification.innerText = "Click on the map to add a crime alert";
                notification.style.position = "absolute";
                notification.style.top = "50%";
                notification.style.left = "50%";
                notification.style.transform = "translate(-50%, -50%)";
                notification.style.backgroundColor = "white";
                notification.style.padding = "10px";
                notification.style.border = "1px solid black";
                document.body.appendChild(notification);

                // add event listener to image for placing marker
                const image = document.querySelector(".image-container img");
                image.addEventListener("click", function (event) {
                    const imageRect = image.getBoundingClientRect();
                    const xCoordinate = event.clientX - imageRect.left;
                    const yCoordinate = event.clientY - imageRect.top;

                    const x2Coordinate = event.clientX - event.target.offsetLeft;
                    const y2Coordinate = event.clientY - event.target.offsetTop;

                    // add a red dot at the location of the click
                    const dot = document.createElement("div");
                    dot.style.width = "10px";
                    dot.style.height = "10px";
                    dot.style.borderRadius = "50%";
                    dot.style.backgroundColor = "red";
                    dot.style.position = "absolute";
                    dot.style.top = y2Coordinate + "px";
                    dot.style.left = x2Coordinate + "px";
                    crimeMapContent.appendChild(dot);

                    AddAlert(homepageContent, xCoordinate, yCoordinate, username);
                    imageContainer.removeEventListener("click", arguments.callee);
                    notification.remove();
                });
            });
            const header = document.querySelector('header');
            header.appendChild(addCrimeAlertButton);
        });
}

async function loadImageContainer(homepageContent, crimeMapContent, username, userrole) {
    const imageContainer = document.createElement("div");
    imageContainer.classList.add("image-container");
    // make container
    const image = document.createElement("img");
    image.src = "./images/crimemap.png";
    image.alt = "Crime map";
    imageContainer.appendChild(image);
    imageContainer.style.overflow = "auto";
    imageContainer.style.width = "calc(100% - 200px)";
    imageContainer.style.height = "calc(100% - 200px)";
    image.style.transform = "scale(1)";
    image.style.transition = "transform 0.5s";

    // add panning
    let isDragging = false;
    let lastX, lastY;
    image.addEventListener("mousedown", function (event) {
        event.preventDefault();
        isDragging = true;
        lastX = event.clientX;
        lastY = event.clientY;
    });
    image.addEventListener("mousemove", function (event) {
        event.preventDefault();
        if (isDragging) {
            const deltaX = event.clientX - lastX;
            const deltaY = event.clientY - lastY;
            lastX = event.clientX;
            lastY = event.clientY;
            imageContainer.scrollLeft -= deltaX;
            imageContainer.scrollTop -= deltaY;
        }
    });
    image.addEventListener("mouseup", function (event) {
        event.preventDefault();
        isDragging = false;
    });

    crimeMapContent.appendChild(imageContainer);

    // mark alerts on map
    markAlertsOnMap(homepageContent, imageContainer, username, userrole);

    return imageContainer;
}


async function getAlerts() {
    try {
        const response = await fetch(api+'/CrimeAlert/getAlerts');
        if (response.ok) {
            const data = await response.json();
            return data;
        } else {
            throw new Error('Error');
        }
    } catch {
        throw new Error('Error');
    }
}

async function markAlertsOnMap(homepageContent, imageContainer, username, userrole) {
    const alerts = await getAlerts();
    // make buttons
    alerts.forEach(alert => {
        const button = document.createElement('button');
        button.classList.add('dot');
        button.style.position = 'absolute';
        button.style.width = '10px';
        button.style.height = '10px';
        button.style.backgroundColor = 'red';
        button.style.borderRadius = '50%';
        button.style.left = `${alert.x}px`;
        button.style.top = `${alert.y}px`;
        button.setAttribute('id', alert.id);
        imageContainer.appendChild(button);

        if (alert.email == username) {
             button.addEventListener('click', () => {
                 EditAlert(homepageContent, alert, username, userrole);
             });
        } else {
            button.addEventListener('click', () => {
                ViewAlert(alert);
            });
        }
    });
}

async function EditAlert(homepageContent, alert, username, userrole) {
    if (userrole !== "Property Manager") {
        loadCrimeMapPage(homepageContent, username, userrole);
    }
    const popup = document.createElement('div');
    popup.style.position = 'absolute';
    popup.style.top = '50%';
    popup.style.left = '50%';
    popup.style.transform = 'translate(-50%, -50%)';
    popup.style.backgroundColor = 'white';
    popup.style.padding = '20px';
    popup.style.borderRadius = '10px';
    popup.style.boxShadow = '0 0 10px rgba(0, 0, 0, 0.5)';
    popup.style.zIndex = '1';

    const closeButton = document.createElement('button');
    closeButton.innerHTML = 'X';
    closeButton.style.position = 'absolute';
    closeButton.style.top = '15px';
    closeButton.style.right = '15px';
    closeButton.style.backgroundColor = 'red';
    closeButton.addEventListener('click', () => {
        popup.remove();
    });
    popup.appendChild(closeButton);

    const form = document.createElement('form');
    form.style.display = 'flex';
    form.style.flexDirection = 'column';
    form.style.gap = '10px';
    form.style.alignSelf = 'right';

    const nameLabel = document.createElement('label');
    nameLabel.innerHTML = 'Name:';
    const nameInput = document.createElement('input');
    nameInput.type = 'text';
    nameInput.value = alert.name;
    form.appendChild(nameLabel);
    form.appendChild(nameInput);

    const locationLabel = document.createElement('label');
    locationLabel.innerHTML = 'Location:';
    const locationInput = document.createElement('input');
    locationInput.type = 'text';
    locationInput.value = alert.location;
    form.appendChild(locationLabel);
    form.appendChild(locationInput);

    const descriptionLabel = document.createElement('label');
    descriptionLabel.innerHTML = 'Description:';
    const descriptionInput = document.createElement('textarea');
    descriptionInput.rows = '5';
    descriptionInput.value = alert.description;
    form.appendChild(descriptionLabel);
    form.appendChild(descriptionInput);

    const timeLabel = document.createElement('label');
    timeLabel.innerHTML = 'Time:';
    const timeInput = document.createElement('input');
    timeInput.type = 'time';
    timeInput.value = alert.time;
    form.appendChild(timeLabel);
    form.appendChild(timeInput);

    const dateLabel = document.createElement('label');
    dateLabel.innerHTML = 'Date:';
    const dateInput = document.createElement('input');
    dateInput.type = 'date';
    dateInput.value = alert.date;
    form.appendChild(dateLabel);
    form.appendChild(dateInput);

    const saveButton = document.createElement('savebtn');
    saveButton.innerHTML = 'Save';
    saveButton.type = 'button';
    form.appendChild(saveButton);

    const deleteButton = document.createElement('deletebtn');
    deleteButton.innerHTML = 'Delete';
    deleteButton.type = 'button';
    form.appendChild(deleteButton);

    popup.appendChild(form);
    document.body.appendChild(popup);

    saveButton.addEventListener('click', async () => {
        const name = nameInput.value;
        const location = locationInput.value;
        const description = descriptionInput.value;
        const time = timeInput.value;
        const date = dateInput.value;

        const newAlert = {
            Email: username,
            ID: alert.id,
            Name: name,
            Location: location,
            Description: description,
            Time: time,
            Date: date,
            X: alert.x,
            Y: alert.y,
        };

        try {
            const response = await fetch(api + '/CrimeAlert/editAlert', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(newAlert),
            });

            if (!response.ok) {
                throw new Error('Error');
            }
            popup.remove();
            loadCrimeMapPage(homepageContent, username, userrole);
            const result = await response.json();
            console.log(result.message);
        } catch (error) {
            console.error(error);
        }
        popup.remove();
        loadCrimeMapPage(homepageContent, username, userrole);
    });

    deleteButton.addEventListener('click', async () => {
        try {
            const data = {
                Email: username,
                ID: alert.id,
            };
            const response = await fetch(api + '/CrimeAlert/DeleteAlert', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(data),
            });

            if (!response.ok) {
                throw new Error('Error');
            }
            
            const result = await response.json();
            console.log(result.message);
        } catch (error) {
            console.error(error);
        }
        popup.remove();
        loadCrimeMapPage(homepageContent, username, userrole);
    });
}

async function AddAlert(homepageContent, xCoordinate, yCoordinate, username) {
    const popup = document.createElement('div');
    popup.style.position = 'absolute';
    popup.style.top = '50%';
    popup.style.left = '50%';
    popup.style.transform = 'translate(-50%, -50%)';
    popup.style.backgroundColor = 'white';
    popup.style.padding = '20px';
    popup.style.borderRadius = '10px';
    popup.style.boxShadow = '0 0 10px rgba(0, 0, 0, 0.5)';
    popup.style.zIndex = '1';

    const closeButton = document.createElement('button');
    closeButton.innerHTML = 'X';
    closeButton.style.position = 'absolute';
    closeButton.style.top = '15px';
    closeButton.style.right = '15px';
    closeButton.style.backgroundColor = 'red';
    closeButton.addEventListener('click', () => {
        popup.remove();
    });
    popup.appendChild(closeButton);

    const form = document.createElement('form');
    form.style.display = 'flex';
    form.style.flexDirection = 'column';
    form.style.gap = '10px';
    form.style.alignSelf = 'right';

    const nameLabel = document.createElement('label');
    nameLabel.innerHTML = 'Name:';
    const nameInput = document.createElement('input');
    nameInput.type = 'text';
    form.appendChild(nameLabel);
    form.appendChild(nameInput);

    const locationLabel = document.createElement('label');
    locationLabel.innerHTML = 'Location:';
    const locationInput = document.createElement('input');
    locationInput.type = 'text';
    form.appendChild(locationLabel);
    form.appendChild(locationInput);

    const descriptionLabel = document.createElement('label');
    descriptionLabel.innerHTML = 'Description:';
    const descriptionInput = document.createElement('textarea');
    descriptionInput.rows = '5';
    form.appendChild(descriptionLabel);
    form.appendChild(descriptionInput);

    const timeLabel = document.createElement('label');
    timeLabel.innerHTML = 'Time:';
    const timeInput = document.createElement('input');
    timeInput.type = 'time';
    form.appendChild(timeLabel);
    form.appendChild(timeInput);

    const dateLabel = document.createElement('label');
    dateLabel.innerHTML = 'Date:';
    const dateInput = document.createElement('input');
    dateInput.type = 'date';
    form.appendChild(dateLabel);
    form.appendChild(dateInput);

    const saveButton = document.createElement('addbtn');
    saveButton.innerHTML = 'Add New Alert';
    saveButton.type = 'button';
    form.appendChild(saveButton);

    popup.appendChild(form);
    document.body.appendChild(popup);

    saveButton.addEventListener('click', async () => {
        const name = nameInput.value;
        const location = locationInput.value;
        const description = descriptionInput.value;
        const time = timeInput.value;
        const date = dateInput.value;

        const newAlert = {
            Email: username,
            Name: name,
            Location: location,
            Description: description,
            Time: time,
            Date: date,
            X: xCoordinate,
            Y: yCoordinate
        };
        try {
            const response = await fetch(api + '/CrimeAlert/addAlert', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(newAlert),
            });

            if (!response.ok) {
                throw new Error('Error');
            }
            
            const result = await response.json();
            console.log(result.message);
        } catch (error) {
            console.error(error);
        }
        popup.remove();
        loadCrimeMapPage(homepageContent, username, "Property Manager")
    });
}

function ViewAlert(alert) {
    const popup = document.createElement('div');
    popup.style.position = 'absolute';
    popup.style.top = '50%';
    popup.style.left = '50%';
    popup.style.transform = 'translate(-50%, -50%)';
    popup.style.backgroundColor = 'white';
    popup.style.padding = '20px';
    popup.style.borderRadius = '10px';
    popup.style.boxShadow = '0 0 10px rgba(0, 0, 0, 0.5)';
    popup.style.zIndex = '1';

    const closeButton = document.createElement('button');
    closeButton.innerHTML = 'X';
    closeButton.style.position = 'absolute';
    closeButton.style.top = '15px';
    closeButton.style.right = '15px';
    closeButton.style.backgroundColor = 'red';
    closeButton.addEventListener('click', () => {
        popup.remove();
    });
    popup.appendChild(closeButton);

    const nameParagraph = document.createElement('p');
    nameParagraph.innerHTML = `Name: ${alert.name}`;
    popup.appendChild(nameParagraph);

    const locationParagraph = document.createElement('p');
    locationParagraph.innerHTML = `Location: ${alert.location}`;
    popup.appendChild(locationParagraph);

    const descriptionParagraph = document.createElement('p');
    descriptionParagraph.innerHTML = `Description: ${alert.description}`;
    popup.appendChild(descriptionParagraph);

    const timeParagraph = document.createElement('p');
    timeParagraph.innerHTML = `Time: ${alert.time}`;
    popup.appendChild(timeParagraph);

    const dateParagraph = document.createElement('p');
    dateParagraph.innerHTML = `Date: ${alert.date}`;
    popup.appendChild(dateParagraph);

    document.body.appendChild(popup);
}
