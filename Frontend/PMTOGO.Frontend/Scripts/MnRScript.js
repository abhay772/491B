function LoadMnRPage(homepageContent) 
{ 
  fetch('./Views/MnRMainPage.html')
    .then(response => response.text())
    .then(data => {
      // Handle the response data
      homepageContent.innerHTML = data;

      const projectBtn = document.getElementById("projects-btn");

      // Add event to Load Projects Page
      projectBtn.addEventListener('click', () => {
        LoadProjectsPage(homepageContent);
      });

      const compareServicesBtn = document.getElementById("compare-services-btn");

      // Add event to Load Compare Services Page
      compareServicesBtn.addEventListener('click', () => {
        LoadComapreServicesPage(homepageContent);
      });

      const estimateProjectBtn = document.getElementById("estimate-project-btn");

      // Add event to Load Estimate Project Page
      estimateProjectBtn.addEventListener('click', () => {
        LoadEstimateProjectPage(homepageContent);
      });

      const priceChartBtn = document.getElementById("price-charts-btn");

      // Add event to Load Price Chart Page
      priceChartBtn.addEventListener('click', () => {
        LoadPriceChartsPage(homepageContent);
      });

    })   
    .catch(error => console.error(error));
    
}

function LoadProjectsPage(homepageContent)
{
    fetch('./Views/MnRProjectsPage.html')
    .then(response => response.text())
    .then(data => {
      // Handle the response data
      homepageContent.innerHTML = data;

      
    })
}