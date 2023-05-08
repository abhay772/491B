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
            await getDIYDashboard(homepageContent, username);

            // add button to upload DIY info and video
            const addDIYBtn = document.createElement("button");
            addDIYBtn.innerText = "Add DIY";
            addDIYBtn.addEventListener("click", async () => {
                await uploadDIYInfo();
                await uploadDIYVideo();
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


async function uploadDIYInfo(form) {
    const email = form.elements.email.value;
    const name = form.elements.name.value;
    const description = form.elements.description.value;

    try {
        const response = await fetch(api + '/DIY/uploadinfo', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ email, name, description }),
        });
        if (!response.ok) {
            throw new Error('Error');
        }
        const result = await response.json();
        console.log(result.message);
        // display success message
        form.reset();
        const message = document.createElement('div');
        message.textContent = 'DIY information uploaded successfully.';
        message.classList.add('success-message');
        form.appendChild(message);
    } catch (error) {
        console.error(error);
        // display error message
        const message = document.createElement('div');
        message.textContent = 'Failed to upload DIY information. Please try again later.';
        message.classList.add('error-message');
        form.appendChild(message);
    }
}

async function uploadDIYVideo(form) {
    const email = form.elements.email.value;
    const name = form.elements.name.value;
    const videoFile = form.elements.video.files[0];

    try {
        const formData = new FormData();
        formData.append('email', email);
        formData.append('name', name);
        formData.append('videoFile', videoFile);

        const response = await fetch(api + '/DIY/uploadvideo', {
            method: 'POST',
            body: formData,
        });
        if (!response.ok) {
            throw new Error('Error');
        }
        const result = await response.json();
        console.log(result.message);
        // display success message
        form.reset();
        const message = document.createElement('div');
        message.textContent = 'DIY video uploaded successfully.';
        message.classList.add('success-message');
        form.appendChild(message);
    } catch (error) {
        console.error(error);
        // display error message
        const message = document.createElement('div');
        message.textContent = 'Failed to upload DIY video. Please try again later.';
        message.classList.add('error-message');
        form.appendChild(message);
    }
}

async function addDIY(homepageContent, crimeMapContent, username, userrole)
{
    //add stuff here
    try {
        const response = await fetch(api + '/DIY/addDIY', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
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
}

//create rest of functions below