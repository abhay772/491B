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

            .perform(function () {
                duration = Date.now();
            })
            .waitForElementVisible('homepage', function() {
                duration = (Date.now() - duration)/1000;
            })
            .perform(function() {
                if(duration > 5){
                    throw new Error('NFR failed')
                }
            })
            .assert.urlContains("/homepage.html")
    });

    after(browser => browser.end());

});

module.exports = {
    'Accept request test': function(browser) {
        browser
        .url("http://localhost:6000/")
        .waitForElementVisible('form',5000)
        .click('button[type="submit"]')
        .waitForElementVisible('#property-profile', 5000) 
        .setValue('input[name="noOfBedrooms"]', 1)
        .setValue('input[name="noOfBathrooms"]', 1)
        .setValue('input[name="sqFeet"]', 1)
        .setValue('input[name="Address1"]', "Somewhere somehow")
        .setValue('input[name="Address2"]', "#491")
        .setValue('input[name="City"]', "Long Beach")
        .setValue('input[name="State"]', "CA")
        .setValue('input[name="Zip"]', "90815")
        .setValue('input[name="Desciption"]', 
        "Just testing the description.")
        .click('#evaluate-property-btn')
        .waitForElementVisible('#property-evaluation',5000)
        .assert.notEqual(browser.getText('#property-evaluation'),'00000.00') 
        .end();
    }
}

