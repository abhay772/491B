const {client} = require('nightwatch');

module.exports = {
    'Form submission test': function(browser) {
        browser
        .url("http://localhost:6000/")
        .waitForElementVisible('form',5000)
        .setValue('input[name="email"]', 'user@example.com')
        .setValue('input[name="password"]', 'password123')
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

module.exports = {
    'Autoload Profile test': function(browser) {
        browser
        .url("http://localhost:6000/")
        .waitForElementVisible('form',5000)
        .setValue('input[name="email"]', 'user@example.com')
        .setValue('input[name="password"]', 'password123')
        .click('button[type="submit"]')
        .waitForElementVisible('#property-profile', 5000) 
        .assert.value('input[name="noOfBedrooms"]', 1) // Check if the value of noOfBedrooms input field is 1
        .assert.value('input[name="noOfBathrooms"]', 1) // Check if the value of noOfBathrooms input field is 1
        .assert.value('input[name="sqFeet"]', 1) // Check if the value of sqFeet input field is 1
        .assert.value('input[name="Address1"]', 'Somewhere somehow') // Check if the value of Address1 input field is "Somewhere somehow"
        .assert.value('input[name="Address2"]', '#491') // Check if the value of Address2 input field is "#491"
        .assert.value('input[name="City"]', 'Long Beach') // Check if the value of City input field is "Long Beach"
        .assert.value('input[name="State"]', 'CA') // Check if the value of State input field is "CA"
        .assert.value('input[name="Zip"]', '90815') // Check if the value of Zip input field is "90815"
        .assert.value('input[name="Description"]', 'This is a description.') // Check if the value of Description input field is "This is a description."
        .end();
    }
}