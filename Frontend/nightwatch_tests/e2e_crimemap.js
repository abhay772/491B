const { client } = require('nightwatch');

describe('Login Success', function () {
    before(browser => browser.navigateTo('http://localhost:8080'));

    it('Login with test user', function (browser) {
        let duration;

        browser
            .setValue('input[type=text]', 'test@gmail.com')
            .setValue('input[type=password]', 'testtest')
            .pause(1000)
            .click('button[class="loginbtn"]')

            .perform(function () {
                duration = Date.now();
            })
            .waitForElementVisible('#homepage', function () {
                duration = (Date.now() - duration) / 1000;
            })
            .assert.visible('#crimemap')
            .click('#crimemap')
            .pause(1000)
            .assert.visible('#addAlertButton')
            .click('#addAlertButton')
            .pause(3000)
            .waitForElementVisible('.image-container', 3000)
            .click('.image-container img')
            .pause(1000)
            .waitForElementVisible('div[style="position: absolute; top: 50%; left: 50%; transform: translate(-50%, -50%); background-color: white; padding: 20px; border-radius: 10px; box-shadow: rgba(0, 0, 0, 0.5) 0px 0px 10px; z-index: 1;"]', 5000)
            .pause(1000)
            .setValue('input[type="text"]', 'John Doe')
            .pause(1000)
            .setValue('input[type="text"]:nth-of-type(2)', 'New York')
            .pause(1000)
            .setValue('textarea', 'This is a test description.')
            .pause(1000)
            .setValue('input[type="time"]', '09:00PM')
            .pause(1000)
            .setValue('input[type="date"]', '05-01-2023')
            .pause(1000)
            .click('addbtn')
            .pause(1000)
            .waitForElementVisible('button.dot:nth-of-type(n+1):nth-of-type(-n+200)', 5000)
            .click('button.dot:nth-of-type(n+1):nth-of-type(-n+200)')
            .pause(1000)
            .setValue('input[type="text"]', 'New Name')
            .pause(1000)
            .setValue('input[type="text"]:nth-of-type(2)', 'New City')
            .pause(1000)
            .setValue('textarea', 'New Description')
            .pause(1000)
            .setValue('input[type="time"]', '12:00PM')
            .pause(1000)
            .setValue('input[type="date"]', '01-01-2023')
            .click('savebtn')
            .pause(4000)
            .waitForElementVisible('button.dot:nth-of-type(n+1):nth-of-type(-n+200)', 5000)
            .click('button.dot:nth-of-type(n+1):nth-of-type(-n+200)')
            .pause(10000)
            .click('deletebtn')
            .pause(10000)
            
    });
    after(browser => browser.end());
});