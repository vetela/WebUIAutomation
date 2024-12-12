Feature: User Navigation and Functionality Verification
  As a user
  I want to navigate the website, search for content, and change the language
  So that I can ensure the site functions correctly and meets my needs.

  Scenario: Verifying the About Page loads correctly
    Given the user is on the Home Page
    When they navigate to the About Page
    Then the URL should be "https://en.ehu.lt/about/"
    And the page title should be "About"
    And the header text should be "About"

  Scenario Outline: Verifying Search Functionality
    Given the user is on the Home Page
    When they search for "<searchTerm>"
    Then the URL should contain "/?s=<searchTerm>"
    And the search results should include "<searchTerm>"

    Examples:
      | searchTerm    |
      | research      |
      | admissions    |

  Scenario: Verifying language change to Lithuanian
    Given the user is on the Home Page
    When they switch the language to Lithuanian
    Then the URL should be "https://lt.ehu.lt/"
    And the lang attribute of the page should be "lt-LT"
