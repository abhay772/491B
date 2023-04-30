const api = "https://localhost:7135/api"

async function loadCrimeMapPage(homepageContent, username) {
    // fetch crime map page html
    fetch("./Views/crimeMap.html")
    .then((response) => response.text())
    .then(async (data) => {
        // update homepage content div with crime map page html
        homepageContent.innerHTML = data;

        const crimeMapContent = document.getElementById("crime-map-content");

        // add crime map 
        const imageContainer = await loadImageContainer(crimeMapContent, username);

        // add "Add Crime Alert" button
        const addCrimeAlertButton = document.createElement("registrationbtn");
        addCrimeAlertButton.textContent = "Add Crime Alert";
        addCrimeAlertButton.addEventListener("click", function () {
            loadAddAlertPage(homepageContent, username);
        });
        crimeMapContent.appendChild(addCrimeAlertButton);

        
    });
}

async function loadImageContainer(crimeMapContent, username) {
    const imageContainer = document.createElement("div");
    imageContainer.classList.add("image-container");

    const image = document.createElement("img");
    image.src = "./images/crimemap.png";
    image.alt = "Crime map";
    imageContainer.appendChild(image);

    // add styles to image container
    imageContainer.style.overflow = "auto";
    imageContainer.style.width = "800px";
    imageContainer.style.height = "800px";
    image.style.transform = "scale(1)";
    image.style.transition = "transform 0.5s";

    // add event listener to image for panning
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
    markAlertsOnMap(imageContainer, username);

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

async function markAlertsOnMap(imageContainer, username) {
    // get alerts from API
    const alerts = await getAlerts();

    // iterate through alerts and add buttons to map
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
        imageContainer.appendChild(button);
        console.log(`Marked alert: ${alert.id}`);

        // add click event listener to button
        button.addEventListener('click', () => {
            // create popup
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

            // add close button to popup
            const closeButton = document.createElement('button');
            closeButton.innerHTML = 'X';
            closeButton.style.position = 'absolute';
            closeButton.style.top = '5px';
            closeButton.style.right = '5px';
            closeButton.addEventListener('click', () => {
                popup.remove();
            });
            popup.appendChild(closeButton);

            // add form to popup
            const form = document.createElement('form');
            form.style.display = 'flex';
            form.style.flexDirection = 'column';
            form.style.gap = '10px';
            form.style.alignSelf = 'right';

            // add input fields to form
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
            timeInput.type = 'text';
            form.appendChild(timeLabel);
            form.appendChild(timeInput);

            const dateLabel = document.createElement('label');
            dateLabel.innerHTML = 'Date:';
            const dateInput = document.createElement('input');
            dateInput.type = 'date';
            form.appendChild(dateLabel);
            form.appendChild(dateInput);

            // add save button to form
            const saveButton = document.createElement('addbtn');
            saveButton.innerHTML = 'Save';
            saveButton.type = 'button'; // change to button type
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

                    const data = await response.json();
                    console.log(data.message);
                } catch (error) {
                    console.error(error);
                }
            });
        });
    });
}

function loadAddAlertPage(homepageContent, username) {
    // fetch add alert page html
    fetch('./Views/addAlert.html')
        .then(response => response.text())
        .then(data => {
            // update homepage content div with add alert page html
            homepageContent.innerHTML = data;

            const crimeMapImage = document.createElement('img');
            crimeMapImage.src = './images/crimemap.png';
            crimeMapImage.alt = 'Crime map';

            const crimeMapContainer = document.createElement('div');
            crimeMapContainer.classList.add('image-container');
            crimeMapContainer.style.overflow = 'auto';
            crimeMapContainer.style.width = '750px';
            crimeMapContainer.style.height = '750px';
            crimeMapContainer.appendChild(crimeMapImage);

            const addAlertContainer = document.getElementById('add-alert-container');
            addAlertContainer.appendChild(crimeMapContainer);

            const marker = document.createElement('div');
            marker.classList.add('marker');
            crimeMapContainer.appendChild(marker);

            let isPlacingMarker = true;
            let xCoordinate = '';
            let yCoordinate = '';

            // add event listener to image for placing marker
            crimeMapImage.addEventListener('click', function addMarker(event) {
                if (isPlacingMarker) {
                    marker.style.display = 'block';
                    marker.style.left = (event.clientX - crimeMapContainer.getBoundingClientRect().left + crimeMapContainer.scrollLeft - 10) + 'px';
                    marker.style.top = (event.clientY - crimeMapContainer.getBoundingClientRect().top + crimeMapContainer.scrollTop - 10) + 'px';

                    // log location in pixels
                    xCoordinate = (event.clientX - crimeMapContainer.getBoundingClientRect().left + crimeMapContainer.scrollLeft).toString();
                    yCoordinate = (event.clientY - crimeMapContainer.getBoundingClientRect().top + crimeMapContainer.scrollTop).toString();

                    isPlacingMarker = false;
                }
            });

            const submitButton = document.getElementById('submit-button');
            submitButton.addEventListener('click', function () {
                const nameInput = document.getElementById('name-input').value;
                const locationInput = document.getElementById('location-input').value;
                const descriptionInput = document.getElementById('description-input').value;
                const timeInput = document.getElementById('time-input').value;
                const dateInput = document.getElementById('date-input').value;

                const url = api + '/CrimeAlert/addAlert';
                const data = {
                    Email: username,
                    Name: nameInput,
                    Location: locationInput,
                    Description: descriptionInput,
                    Time: timeInput,
                    Date: dateInput,
                    X: xCoordinate,
                    Y: yCoordinate
                };

                send(url, data)
                    .then(response => {
                        console.log(data);
                        console.log(response);

                        loadCrimeMapPage(homepageContent, username);
                    })
                    .catch(error => console.log(error));
            });
        })
        .catch(error => console.log(error));
}