async function loadDIYPage(homepageContent, username, userrole)
{
    // fetch crime map page html
    fetch("./Views/diy.html")
        .then((response) => response.text())
        .then(async (data) => {
            // update homepage content div with crime map page html
            homepageContent.innerHTML = data;

            if (userrole !== "Property Manager") {
                loadHomePage(userrole, username);
            }

            // call function to get DIY dashboard and display it
            //await getDIYDashboard(homepageContent, username);

            // add button to upload DIY info and video
            const addDIYBtn = document.createElement("button");
            addDIYBtn.innerText = "Add DIY";
            addDIYBtn.addEventListener("click", async () => {
                const addDIYForm = await addDIY(homepageContent);
                document.body.appendChild(addDIYForm);
            });
            homepageContent.appendChild(addDIYBtn);
        });
}

async function getDIYDashboard(homepageContent, username) {
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

        const dashboardContainer = document.createElement('div');
        dashboardContainer.classList.add('dashboard-container');
        homepageContent.appendChild(dashboardContainer);

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

function addDIY(homepageContent) {
    const popupContainer = document.createElement('div');
    popupContainer.classList.add('popup-container');
    const form = document.createElement('form');
    form.classList.add('popup-form');

    // Add file input field for video upload
    const fileInput = document.createElement('input');
    fileInput.setAttribute('type', 'file');
    fileInput.setAttribute('name', 'video');
    fileInput.setAttribute('accept', 'video/*');
    fileInput.required = true;
    form.appendChild(fileInput);

    // Add name input field
    const nameInput = document.createElement('input');
    nameInput.setAttribute('type', 'text');
    nameInput.setAttribute('name', 'name');
    nameInput.setAttribute('placeholder', 'Name');
    nameInput.required = true;
    form.appendChild(nameInput);

    // Add description input field
    const descriptionInput = document.createElement('textarea');
    descriptionInput.setAttribute('name', 'description');
    descriptionInput.setAttribute('placeholder', 'Description');
    descriptionInput.required = true;
    form.appendChild(descriptionInput);

    // Add email input field
    const emailInput = document.createElement('input');
    emailInput.setAttribute('type', 'email');
    emailInput.setAttribute('name', 'email');
    emailInput.setAttribute('placeholder', 'Email');
    emailInput.required = true;
    form.appendChild(emailInput);

    // Add submit button
    const submitButton = document.createElement('button');
    submitButton.setAttribute('type', 'submit');
    submitButton.textContent = 'Submit';
    form.appendChild(submitButton);

    // Add event listener for form submission
form.addEventListener('submit', async (event) => {
    event.preventDefault();
    const formData = new FormData(event.target);
    const data = {
        email: emailInput,
        name: nameInput,
        description: descriptionInput,
        videoFile: fileInput
    };


    try {
        // Upload DIY information
        const infoResponse = await fetch(api + '/DIY/UploadInfo', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

        if (!infoResponse.ok) {
            throw new Error('Failed to upload DIY information');
        }

        // Upload video file
        const videoFormData = new FormData();
        videoFormData.append('email', email);
        videoFormData.append('name', name);
        videoFormData.append('videoFile', videoFile, videoFile.name); // Append video file with IFormFile
        const videoResponse = await fetch(api+'/DIY/UploadVideo', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });
        if (!videoResponse.ok) {
            throw new Error('Failed to upload video file');
        }

        // Display success message
        console.log('DIY information and video uploaded successfully');
        form.reset();
        const message = document.createElement('div');
        message.textContent = 'DIY information and video uploaded successfully.';
        message.classList.add('success-message');
        form.appendChild(message);

        // Close popup after successful upload
        setTimeout(() => {
            closePopup();
        }, 2000);
    } catch (error) {
        console.error(error);
        // Display error message
        const message = document.createElement('div');
        message.textContent = 'Failed to upload DIY information and/or video. Please try again later.';
        message.classList.add('error-message');
        form.appendChild(message);
    }
});
homepageContent.appendChild(form);
}