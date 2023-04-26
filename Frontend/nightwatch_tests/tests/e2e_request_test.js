const {client} = require('nightwatch');
//from v-vong/csulb github
describe('Login Success', function() {
    before(browser => browser.navigateTo('http://localhost:8080'));

    it('Login with test user', function(browser){
        let duration;

        browser
            .setValue('input[type=text]', 'mssierra310@gmail.com')
            .setValue('input[type=password', 'randomstring')
            .pause()
            .click('button[class="loginbtn"]')
            .waitForElementVisible('button[type="submit"]', 5000)
            .click('button[type="submit"]')
            .click('.servicerequest-linkc div:nth-child(2) button')
            .waitForElementVisible('#NewRequest', 10000)
            .setValue('input[name="frequencies"]', "month")
            .setValue('input[name="times"]', 3)
            .setValue('input[name="comments"]', "e2e new request test")
            .click('#requestservice')
            .waitForElementVisible('#homepage',5000)

            .click('#serviceManagement')
            .waitForElementVisible('div.userservices', 10000)
            .click('.rate-linkc div:nth-child(1) button')
            .waitForElementVisible('#rate-form', 10000)
            .setValue('input[name="rating"]', 4)
            .click('#rateservice')
            .waitForElementVisible('div.userservices',5000)
            .assert.notEqual(browser.getText('div.userservices'),'00000.00') 
            .end();
            
    });

    after(browser => browser.end());

});


