
const projects = [
    {
        projectName: "Project A",
        serviceIDs: [1, 2, 3],
        startDate: new Date("2023-05-01"),
        endDate: new Date("2023-05-31"),
        serviceTime: "09:00",
        budget: 15000
    },
    {
        projectName: "Project B",
        serviceIDs: [4, 5, 6],
        startDate: new Date("2023-06-01"),
        endDate: new Date("2023-06-30"),
        serviceTime: "14:00",
        budget: 10000
    },
];

function formatDate(date) {
    return date.toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
    });
};

function createCard(project, projects, maxProjects) {
    const card = document.createElement('div');
    card.classList.add('card');
    card.setAttribute('data-project-name', project.projectName); // Set the data attribute here

    const projectName = document.createElement('h3');
    projectName.textContent = project.projectName;

    const budget = document.createElement('p');
    budget.textContent = `Budget: $${project.budget.toFixed(2)}`;

    const dateRange = document.createElement('p');
    dateRange.textContent = `${formatDate(project.startDate)} - ${formatDate(project.endDate)}`;

    card.appendChild(projectName);
    card.appendChild(budget);
    card.appendChild(dateRange);

    card.addEventListener('click', (e) => {
        e.preventDefault();
        showExpandedCard(project);
    });

    // Add delete button to the card
    const deleteButton = document.createElement('button');
    deleteButton.textContent = 'Delete';
    deleteButton.classList.add('delete-button');

    // Add click event listener for the delete button
    deleteButton.addEventListener('click', (event) => {
        // Prevent triggering the click event on the card
        event.stopPropagation();

        // Find the index of the project in the projects array
        const projectIndex = projects.findIndex(p => p.projectName === project.projectName);

        // Remove the project from the projects array
        if (projectIndex > -1) {
            projects.splice(projectIndex, 1);
        }

        // Remove the card from the UI
        card.remove();

        // Show the add project card if there are fewer projects than the maximum allowed
        if (projects.length < maxProjects) {
            document.querySelector('.add-project-card').hidden = false;
        }
    });

    // Add the delete button to the card
    card.appendChild(deleteButton);

    return card;
};

function insertCard(card) {
    const addProjectCard = document.querySelector('.add-project-card');
    addProjectCard.parentNode.insertBefore(card, addProjectCard);
};

function showExpandedCard(project) {
    hideAddProjectForm();

    document.querySelector('.expanded-card .project-name').textContent = `Project Name: ${project.projectName}`;
    document.querySelector('.expanded-card .service-ids').textContent = `Service IDs: ${project.serviceIDs.join(', ')}`;
    document.querySelector('.expanded-card .start-date').textContent = `Start Date: ${formatDate(project.startDate)}`;
    document.querySelector('.expanded-card .end-date').textContent = `End Date: ${formatDate(project.endDate)}`;
    document.querySelector('.expanded-card .service-time').textContent = `Service Time: ${project.serviceTime}`;
    document.querySelector('.expanded-card .budget').textContent = `Budget: $${project.budget.toFixed(2)}`;

    const editButton = document.createElement('button');
    editButton.textContent = 'Edit';
    editButton.classList.add('edit-button');

    const cancelButton = document.createElement('button');
    cancelButton.textContent = 'Cancel';
    cancelButton.classList.add('cancel-button');

    // Remove existing buttons from expanded card
    const existingEditButton = document.querySelector('.expanded-card .edit-button');
    const existingCancelButton = document.querySelector('.expanded-card .cancel-button');
    if (existingEditButton) {
        existingEditButton.remove();
    }
    if (existingCancelButton) {
        existingCancelButton.remove();
    }

    document.querySelector('.expanded-card').appendChild(editButton);
    document.querySelector('.expanded-card').appendChild(cancelButton);

    editButton.addEventListener('click', (e) => {
        e.preventDefault();
        editProject(project);
    });

    document.querySelector('.card-container').hidden = true;
    document.querySelector('.expanded-card').hidden = false;
};

function editProject(project) {

    const form = document.querySelector('.add-project-form');
    form.querySelector('input[name="projectName"]').value = project.projectName;
    form.querySelector('input[name="startDate"]').valueAsDate = project.startDate;
    form.querySelector('input[name="endDate"]').valueAsDate = project.endDate;
    form.querySelector('input[name="serviceTime"]').value = project.serviceTime;
    form.querySelector('input[name="budget"]').valueAsNumber = project.budget;

    hideExpandedCard();

    form.hidden = false;

    const saveButton = form.querySelector('button[type="submit"]');
    saveButton.textContent = 'Save Changes';

    // Remove the addNewProject event listener
    form.removeEventListener('submit', addNewProject);

    // Create a named function for updating the project
    function updateProjectListener(e) {
        e.preventDefault();
        updateProject(project, form);
    }

    // Add the updateProject event listener
    form.addEventListener('submit', (e) => {
        e.preventDefault()
        updateProjectListener();
    });

    const cancelButton = form.querySelector('button[type="button"]');
    cancelButton.addEventListener('click', (e) => {

        e.preventDefault();
        saveButton.textContent = 'Add Project';

        // Remove the updateProject event listener
        form.removeEventListener('submit', updateProjectListener);

        // Add the addNewProject event listener
        form.addEventListener('submit', (e) => {
            e.preventDefault();
            addNewProject();
        });

        hideAddProjectForm();
        showExpandedCard(project);
    });
};

function hideExpandedCard() {
    document.querySelector('.card-container').hidden = false;
    document.querySelector('.expanded-card').hidden = true;
};

function addNewProject(e, projects, maxProjects) {
    e.preventDefault();

    const form = document.querySelector('.add-project-form');

    const newProject = {
        projectName: form.querySelector('input[name="projectName"]').value,
        serviceIDs: [5, 6, 7],
        startDate: new Date(form.querySelector('input[name="startDate"]').value),
        endDate: new Date(form.querySelector('input[name="endDate"]').value),
        serviceTime: form.querySelector('input[name="serviceTime"]').value,
        budget: parseFloat(form.querySelector('input[name="budget"]').value),
    };

    projects.push(newProject);

    if (projects.length >= maxProjects) {
        document.querySelector(".add-project-card").hidden = true;
    }

    form.reset();
    form.hidden = true;
    document.querySelector(".card-container").hidden = false;

    insertCard(createCard(newProject, projects, maxProjects));
    hideAddProjectForm();
};

function createAddProjectCard(projects, maxProjects) {

    const addProjectCard = document.createElement("div");
    addProjectCard.classList.add("card", "add-project-card");

    const plusSign = document.createElement("span");
    plusSign.textContent = "+";
    plusSign.classList.add("plus-sign");

    addProjectCard.appendChild(plusSign);

    if (projects.length >= maxProjects) {
        addProjectCard.hidden = true;
    }

    addProjectCard.addEventListener("click", (e) => {
        e.preventDefault();
        hideAddProjectForm();
        hideExpandedCard();
        document.querySelector(".card-container").hidden = true;
        document.querySelector(".add-project-form").hidden = false;
    });

    return addProjectCard;
};

function createAddProjectForm(maxProjects) {

    hideExpandedCard();

    const form = document.createElement("form");
    form.classList.add("add-project-form");
    form.hidden = true;

    const addProjectHeader = document.createElement("h3");
    addProjectHeader.textContent = "Enter project detail:";
    form.appendChild(addProjectHeader);

    // Add your form fields and a submit button
    const projectNameLabel = document.createElement("label");
    projectNameLabel.textContent = "Project Name:";
    const projectNameInput = document.createElement("input");
    projectNameInput.type = "text";
    projectNameInput.name = "projectName";
    projectNameInput.required = true;
    form.appendChild(projectNameLabel);
    form.appendChild(projectNameInput);

    // Add your form fields and a submit button
    const startDateLabel = document.createElement("label");
    startDateLabel.textContent = "Start Date:";
    const startDateInput = document.createElement("input");
    startDateInput.type = "date";
    startDateInput.name = "startDate";
    startDateInput.required = true;
    form.appendChild(startDateLabel);
    form.appendChild(startDateInput);

    // Set the minimum value for the start date input to today's date
    const today = new Date();
    const todayFormatted = today.toISOString().substr(0, 10); // Convert date to YYYY-MM-DD format
    startDateInput.min = todayFormatted;
   
    // Add your form fields and a submit button
    const endDateLabel = document.createElement("label");
    endDateLabel.textContent = "End Date:";
    const endDateInput = document.createElement("input");
    endDateInput.type = "date";
    endDateInput.name = "endDate";
    endDateInput.required = true;
    form.appendChild(endDateLabel);
    form.appendChild(endDateInput);

    startDateInput.addEventListener("change", function () {
        endDateInput.min = startDateInput.value;
    });

    // User cannot set a endDate before setting a startDate
    endDateInput.addEventListener("change", function () {
        if (startDateInput.value === '') {
            alert('Please select a start date first.');
            endDateInput.value = '';
        }
    });

    // Add your form fields and a submit button
    const serviceTimeLabel = document.createElement("label");
    serviceTimeLabel.textContent = "Service Time:";
    const serviceTimeInput = document.createElement("input");
    serviceTimeInput.type = "time";
    serviceTimeInput.name = "serviceTime";
    serviceTimeInput.required = true;
    form.appendChild(serviceTimeLabel);
    form.appendChild(serviceTimeInput);

    // Add budget field
    const budgetLabel = document.createElement("label");
    budgetLabel.textContent = "Budget:";
    const budgetInput = document.createElement("input");
    budgetInput.type = "number";
    budgetInput.name = "budget";
    budgetInput.required = true;
    form.appendChild(budgetLabel);
    form.appendChild(budgetInput);

    const submitButton = document.createElement("button");
    submitButton.type = "submit";
    submitButton.textContent = "Add Project";
    form.appendChild(submitButton);

    form.addEventListener('submit', (e) => { 
        e.preventDefault(); 
        addNewProject(e, projects, maxProjects)
    });

    // Create a cancel button
    const cancelButton = document.createElement("button");
    cancelButton.type = "button";
    cancelButton.textContent = "Cancel";

    // Append the cancel button to the form
    form.appendChild(cancelButton);

    form.classList.add("add-project-form");

    return form;
};

function hideAddProjectForm() {
    document.querySelector('.add-project-form').hidden = true;
};

function updateProject(project, form) {
    const updatedProject = {
        projectName: form.querySelector('input[name="projectName"]').value,
        serviceIDs: project.serviceIDs,
        startDate: new Date(form.querySelector('input[name="startDate"]').value),
        endDate: new Date(form.querySelector('input[name="endDate"]').value),
        serviceTime: form.querySelector('input[name="serviceTime"]').value,
        budget: parseFloat(form.querySelector('input[name="budget"]').value),
    };

    Object.assign(project, updatedProject);

    const card = document.querySelector(`.card[data-project-name="${project.projectName}"]`);
    card.querySelector('h3').textContent = project.projectName;
    card.querySelector('p:nth-of-type(1)').textContent = `Budget: $${project.budget.toFixed(2)}`;
    card.querySelector('p:nth-of-type(2)').textContent = `${formatDate(project.startDate)} - ${formatDate(project.endDate)}`;

    form.reset();
    form.hidden = true;

    document.querySelector('.add-project-form button[type="submit"]').textContent = 'Add Project';
    document.querySelector('.add-project-form').addEventListener('submit', (e) => {
        e.preventDefault();
        addNewProject(e, projects, maxProjects)
    });
    showExpandedCard(project);
};

function initializeProjectsPage(homepageContent) {

    const maxProjects = 3;

    const addProjectCardElement = createAddProjectCard(projects, maxProjects);
    document.querySelector('.card-container').appendChild(addProjectCardElement);

    projects.forEach(project => {
        insertCard(createCard(project, projects, maxProjects));
    });

    document.querySelector(".back-button").addEventListener("click", hideExpandedCard);

    homepageContent.appendChild(createAddProjectForm(maxProjects));

    hideAddProjectForm();
}

initializeProjectsPage();
