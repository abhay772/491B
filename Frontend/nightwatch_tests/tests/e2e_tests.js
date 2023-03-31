const {client} = require('nightwatch');

module.exports = {
    'Form submission test': function(browser) {
        browser
        .url("http://localhost:8080/")
        .waitForElementVisible('form',5000)
        .setValue('input[name="email"]', 'lutherFercia@gmail.com')
        .setValue('input[name="password"]', '123456789')
        .waitForElementVisible('button[type="submit"]', 5000)
        .click('button[type="submit"]')
        .click('#propertyEvaluation')
        .waitForElementVisible('#PropertyProfile', 10000) 
        .setValue('input[name="NoOfBedrooms"]', 1)
        .setValue('input[name="NoOfBathrooms"]', 1)
        .setValue('input[name="SqFeet"]', 1)
        .setValue('input[name="Address1"]', "Somewhere somehow")
        .setValue('input[name="Address2"]', "#491")
        .setValue('input[name="City"]', "Long Beach")
        .setValue('input[name="State"]', "CA")
        .setValue('input[name="Zip"]', "90815")
        //.setValue('input[name="Description"]', "Just testing the description.")
        .click('#evaluate-btn')
        .waitForElementVisible('#property-evaluation',5000)
        .assert.notEqual(browser.getText('#property-evaluation'),'00000.00') 
        .end();
    },



    'Autoload Profile test': function(browser) {
        browser
        .url("http://localhost:8080/")
        .waitForElementVisible('form',5000)
        .setValue('input[name="email"]', 'lutherFercia@gmail.com')
        .setValue('input[name="password"]', '123456789')
        .click('button[type="submit"]')
        .click('#propertyEvaluation')
        .waitForElementVisible('#PropertyProfile', 5000) 
        .assert.valueEquals('input[name="NoOfBedrooms"]', "1") // Check if the value of noOfBedrooms input field is 1
        .assert.valueEquals('input[name="NoOfBathrooms"]', "1") // Check if the value of noOfBathrooms input field is 1
        .assert.valueEquals('input[name="SqFeet"]', "1") // Check if the value of sqFeet input field is 1
        .assert.valueEquals('input[name="Address1"]', 'Somewhere somehow') // Check if the value of Address1 input field is "Somewhere somehow"
        .assert.valueEquals('input[name="Address2"]', '#491') // Check if the value of Address2 input field is "#491"
        .assert.valueEquals('input[name="City"]', 'Long Beach') // Check if the value of City input field is "Long Beach"
        .assert.valueEquals('input[name="State"]', 'CA') // Check if the value of State input field is "CA"
        .assert.valueEquals('input[name="Zip"]', '90815') // Check if the value of Zip input field is "90815"
        //.assert.valueEquals('input[name="Description"]', 'This is a description.') // Check if the value of Description input field is "This is a description."
        .end();
    }
}
