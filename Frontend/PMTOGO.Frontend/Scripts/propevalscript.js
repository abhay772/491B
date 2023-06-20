// function to load property evaluation page
function loadPropertyEvalPage(homepageContent) {
    // fetch property evaluation page html
    fetch('./Views/propEval.html')
      .then(response => response.text())
      .then(data => {
        // update content div with property evaluation page html
        homepageContent.innerHTML = data;
  
        LoadProfile();
        updateEvaluation();
  
        const evaluateForm = document.getElementById("PropertyProfile");
  
        evaluateForm.addEventListener('submit', function(event) {
          event.preventDefault();
  
          SaveProfile();
          updateEvaluation();
      });
      })
      .catch(error => console.log(error));
  
  }
  
  function updateEvaluation(){
    // Update the evaluation
    url = api + '/PropEval/evaluate';
  
    get(url)
    .then((response) => {
      if (response.ok) {
        return response.text();
      }
    })
    .then(data => {
      document.getElementById("property-evaluation").innerHTML = `Property Evaluation: ${data}`;
    })
    .catch(error => console.error(error));
  }
  
  async function LoadProfile(){
    // Auto loading the Property Profile if available
    url = api + '/PropEval/loadProfile';
  
    get(url)
    .then(response => {
  
      if(!response.ok){
  
        fetch("./Views/NotAuthorized.html")
        .then(response => response.text())
        .then(text => {
  
          const homepageContent = document.getElementsByClassName("homepage-content")[0];
      
          homepageContent.innerHTML = text;
        })
  
      } else {
  
        response.json().then(data => {
          const fieldset = document.querySelector('#PropertyProfile fieldset');
          assignData(fieldset,data);
        }).catch(error => console.error(error));
  
      }
    })
    .catch(error => console.error(error.text));
  }
  
  async function SaveProfile(){
  
    // Saving the Property Profile
    url = api + '/PropEval/saveProfile';
  
    const fieldset = document.querySelector('#PropertyProfile fieldset');
  
    let data = extractData(fieldset);
    console.log(data);
    await put(url,data)
    .catch(error => console.error(error));
  }
  
  function assignData(fieldset, data){
    const inputs = fieldset.querySelectorAll('input, textarea');
    inputs.forEach(input => {
  
      // Lower casing the first letter of the inout field
      // to match the property name in data
      const key = input.name.charAt(0).toLowerCase() + input.name.slice(1);
  
      if(data.hasOwnProperty(key)){
        if(data[key] !== 'string' && data[key] !== 0)
        {
          //console.log(data[key]);
          input.setAttribute('value',data[key]);
        }
      }
    });
  }
  
  function extractData(fieldset){
    const inputs = fieldset.querySelectorAll('input, textarea');
    let data = {};
  
    inputs.forEach(input => {
      const key = input.name.charAt(0).toLowerCase() + input.name.slice(1);
      data[key] = input.value;
    })
    console.log(data)
    return data;
  }