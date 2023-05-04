const { client } = require('nightwatch');

describe('Login Success', function () {
    before(browser => browser.url('http://localhost:8080'));

    it('should login with test user', function (browser) {
        let duration;

        browser
            .setValue('input[type=text]', 'test@gmail.com')
            .setValue('input[type=password]', 'testtest')
            .click('button[class="loginbtn"]')
            .perform(function () {
                duration = Date.now();
            })
            .waitForElementVisible('#homepage', function () {
                duration = (Date.now() - duration) / 1000;
            })
            .perform(function () {
                if (duration > 5) {
                    throw new Error('NFR failed');
                }
            })
            .assert.urlContains('/homepage.html')
            .end();
    });
});