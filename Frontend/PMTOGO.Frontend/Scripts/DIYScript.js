async function loadDIYPage(homepageContent, username, userrole) {
    // fetch crime map page html
    fetch("./Views/diy.html")
        .then((response) => response.text())
        .then(async (data) => {
            // update homepage content div with crime map page html
            homepageContent.innerHTML = data;

            if (userrole !== "Property Manager") {
                console.log("Error Invalid User");
                loadHomePage(userrole, username);
            }

            const diyContent = document.getElementById("diy-content");
            const dashboardContainer = await loadDashboardContainer(homepageContent, diyContent, username, userrole);
            const header = document.querySelector('header');

            // add button to upload DIY info and video
            const addDIYBtn = document.getElementById("addDIYButton");
            addDIYBtn.innerText = "Add DIY";
            addDIYBtn.addEventListener("click", async () => {
                await AddDIY(homepageContent, username, userrole);
            });

            header.appendChild(addDIYBtn);

        });
}

async function AddDIY(homepageContent, username, userrole) {
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

    const descriptionLabel = document.createElement('label');
    descriptionLabel.innerHTML = 'Description:';
    const descriptionInput = document.createElement('textarea');
    descriptionInput.rows = '5';
    form.appendChild(descriptionLabel);
    form.appendChild(descriptionInput);

    const videoLabel = document.createElement('label');
    videoLabel.innerHTML = 'Video:';
    const videoInput = document.createElement('input');
    videoInput.type = 'file';
    form.appendChild(videoLabel);
    form.appendChild(videoInput);

    const saveButton = document.createElement('button');
    saveButton.innerHTML = 'Upload DIY Info';
    form.appendChild(saveButton);

    popup.appendChild(form);
    document.body.appendChild(popup);

    saveButton.addEventListener('click', async () => {
        const formData = new FormData();
        formData.append('email', username);
        formData.append('name', nameInput.value);
        formData.append('description', descriptionInput.value);
        formData.append('videofile', videoInput.files[0]);

        try {
            const response = await fetch(api + '/DIY/uploadDIY', {
                method: 'POST',
                body: formData
            });

            if (!response.ok) {
                throw new Error('Error');
            }

            const result = await response.json();
            console.log(result.message);
            loadDIYPage(homepageContent, username, userrole)
        } catch (error) {
            console.error(error);
        }
        loadDIYPage(homepageContent, username, userrole)
    });
}

async function loadDashboardContainer(homepageContent, diyContent, username, userrole) {
    try {
        const response = await fetch(api + '/DIY/getDashboardDIY', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ username: username })
        });

        if (!response.ok) {
            throw new Error('Failed to load DIY dashboard');
        }
        const data = await response.json();

        console.log(data);

        const dashboardContainer = document.createElement('div');
        dashboardContainer.classList.add('dashboard-container');
        diyContent.appendChild(dashboardContainer);

        // create 5 boxes with DIY object information
        for (let i = 0; i < 5 && i < data.length; i++) {
            const diyObject = data[i];
            const diyBox = document.createElement('div');
            diyBox.classList.add('diy-box');
            dashboardContainer.appendChild(diyBox);

            const name = document.createElement('div');
            name.classList.add('diy-name');
            name.textContent = diyObject.name;
            diyBox.appendChild(name);

            const description = document.createElement('div');
            description.classList.add('diy-description');
            description.textContent = diyObject.description;
            diyBox.appendChild(description);

            const detailsButton = document.createElement('button');
            detailsButton.textContent = 'Details';
            detailsButton.classList.add('diy-details-button');
            detailsButton.addEventListener('click', () => {
                DIYDetails(diyObject.id);
            });
            diyBox.appendChild(detailsButton);
        }
    } catch (error) {
        console.error(error);
    }
}

async function DIYDetails(diyId) {
    try {
        const response = await fetch(api + '/DIY/getVideo', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ id: diyId })
        });

        if (!response.ok) {
            throw new Error('Failed to load DIY video');
        }

        const videoBlob = await response.blob();
        const videoUrl = URL.createObjectURL(videoBlob);

        const popupContainer = document.createElement('div');
        popupContainer.classList.add('diy-popup-container');

        const videoPlayer = document.createElement('video');
        videoPlayer.classList.add('diy-video-player');
        videoPlayer.src = videoUrl;
        videoPlayer.controls = true;

        popupContainer.appendChild(videoPlayer);

        document.body.appendChild(popupContainer);
    } catch (error) {
        console.error(error);
    }
}