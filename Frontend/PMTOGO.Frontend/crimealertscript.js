/*async function updateCrimeMap() {
    const response = await fetch(api + '/CrimeAlert/getAlerts');
    const data = await response.json();
    const markers = [];

    for (const alert of data) {
        const newMarker = document.createElement('div');
        newMarker.classList.add('marker');
        newMarker.style.left = `${alert.X}px`;
        newMarker.style.top = `${alert.Y}px`;
        newMarker.addEventListener('click', function () {
            alert(`Crime Alert: ${alert.description}`);
        });
        markers.push(newMarker);
    }

    return markers;
}*/

async function updateCrimeMap() {
    const response = await fetch(api + '/CrimeAlert/getAlerts');
    const data = await response.json();
    const markers = [];

    for (const alert of data) {
        const newMarker = document.createElement('img');
        newMarker.src = './images/marker.png';
        newMarker.classList.add('marker');
        newMarker.style.position = 'absolute';
        newMarker.style.left = `${alert.X}px`;
        newMarker.style.top = `${alert.Y}px`;
        newMarker.setAttribute('data-alert-id', alert.id);
        newMarker.style.zIndex = 9999; // Add this line to set the z-index
        markers.push(newMarker);
    }

    return markers;
}

function loadCrimeMapPage(homepageContent, username) {
    // fetch crime map page html
    fetch('./Views/crimeMap.html')
        .then(response => response.text())
        .then(data => {
            // update homepage content div with crime map page html
            homepageContent.innerHTML = data;

            // add image to crime map page
            const imageContainer = document.createElement('div');
            imageContainer.classList.add('image-container');

            const image = document.createElement('img');
            image.src = './images/crimemap.png';
            image.alt = 'Crime map';
            imageContainer.appendChild(image);

            const crimeMapContent = document.getElementById('crime-map-content');
            crimeMapContent.appendChild(imageContainer);

            // add styles to image container
            imageContainer.style.overflow = 'auto';
            imageContainer.style.width = '750px';
            imageContainer.style.height = '750px';
            image.style.transform = 'scale(1)';
            image.style.transition = 'transform 0.5s';

            // set initial scroll position
            imageContainer.scrollLeft = 5400;
            imageContainer.scrollTop = 7300;

            // add event listener to image for panning
            let isDragging = false;
            let lastX, lastY;
            image.addEventListener('mousedown', function (event) {
                event.preventDefault();
                isDragging = true;
                lastX = event.clientX;
                lastY = event.clientY;
            });
            image.addEventListener('mousemove', function (event) {
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
            image.addEventListener('mouseup', function (event) {
                event.preventDefault();
                isDragging = false;
            });

            // add "Add Crime Alert" button
            const addCrimeAlertButton = document.createElement('button');
            addCrimeAlertButton.textContent = 'Add Crime Alert';
            addCrimeAlertButton.addEventListener('click', function () {
                loadAddAlertPage(homepageContent, username);
            });
            crimeMapContent.appendChild(addCrimeAlertButton);

            // call updateCrimeMap to get the markers and add them to the map
            updateCrimeMap().then(markers => {
                markers.forEach(marker => {
                    // insert code here to set the pixel location of the marker
                    const left = alert.X;
                    const top = alert.Y;
                    marker.style.left = `${left}px`;
                    marker.style.top = `${top}px`;

                    // append the marker to the image container
                    imageContainer.appendChild(marker);

                    marker.addEventListener('click', function () {
                        alert(`Crime Alert: ${marker.getAttribute('data-alert-id')}`);
                    });
                });
            });
        });
}

/*function updateCrimeMap() {
    // call controller method to get crime alerts
    fetch('https://localhost:7135/api/CrimeAlert/getAlerts')
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(alerts => {
            // iterate through alerts and add markers to the map
            alerts.forEach(alert => {
                const marker = document.createElement('button');
                marker.classList.add('marker');
                marker.style.position = 'absolute';
                marker.style.left = alert.X + 'px';
                marker.style.top = alert.Y + 'px';
                marker.dataset.alert = JSON.stringify(alert);
                marker.addEventListener('click', function () {
                });
                const imageContainer = document.querySelector('.image-container');
                imageContainer.appendChild(marker);
            });
        })
        .catch(error => console.log(error));
}*/

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
            // after successful submission, load crime map page
          loadCrimeMapPage(homepageContent);
        })
        .catch(error => console.log(error));
      });
  })
  .catch(error => console.log(error));
}



/*function loadCrimeMapPage() {
    // fetch crime map page html
    fetch('./Views/crimeMap.html')
        .then(response => response.text())
        .then(data => {

            // add image to crime map page
            const imageContainer = document.createElement('div');
            imageContainer.classList.add('image-container');

            const image = document.createElement('img');
            image.src = './images/crimemap.png';
            image.alt = 'Crime map';
            imageContainer.appendChild(image);

            const crimeMapContent = document.getElementById('crime-map-content');
            crimeMapContent.appendChild(imageContainer);

            // attach event listener to back button
            const backButton = document.getElementById('back');
            if (backButton) {
                backButton.addEventListener('click', () => {
                    loadHomePage();
                });
            }
        })
        .catch(error => console.log(error));
}*/