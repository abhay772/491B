const { client } = require('nightwatch');

describe('Login Success', function () {
    before(browser => browser.navigateTo('http://localhost:8080'));

    it('Login with test user', function (browser) {
        let duration;

        browser
            .setValue('input[type=text]', 'test0@gmail.com')
            .setValue('input[type=password', 'testtest')
            .pause()
            .click('button[class="loginbtn"]')

            .perform(function () {
                duration = Date.now();
            })
            .waitForElementVisible('homepage', function () {
                duration = (Date.now() - duration) / 1000;
            })
            .perform(function () {
                if (duration > 5) {
                    throw new Error('NFR failed')
                }
            })
            .assert.urlContains("/homepage.html")
    });
    after(browser => browser.end());
});