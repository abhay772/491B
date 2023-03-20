const form = document.querySelector('form');

form.addEventListener('submit', event => {
  event.preventDefault();

  const username = document.querySelector('#username').value;
  const password = document.querySelector('#password').value;

  const api = 'https://localhost:7135/api/Authentication/Login';

  const options = {
    method:'POST',
    body: JSON.stringify({ Username: username, Password: password }),
    mode: 'cors',
    credentials: 'include',
    headers: {
      'Content-Type': 'application/json'
    },
    redirect: 'follow',
    referrerPolicy: 'no-referrer-when-downgrade'
  };

  fetch(api, options)
    .then(response => response.text())
    .then(text => console.log(text))
    .catch(error => console.log(error.text()));

});
