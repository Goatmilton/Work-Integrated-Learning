\# Woodlands API — QA Test Report



\## 1. Document Information



| Item              | Details                                                                |

| ----------------- | ---------------------------------------------------------------------- |

| Project           | Woodlands API                                                          |

| QA Role           | Testing \& Quality Assurance Lead                                       |

| Test Framework    | Jest                                                                   |

| API Test Library  | Supertest                                                              |

| Runtime           | Node.js                                                                |

| Test Database/API | Supabase                                                               |

| Branch            | `develop`                                                              |

| Final Test Result | \*\*39/39 tests passing\*\*                                                |

| Final Coverage    | \*\*51.27% statements, 28.65% branches, 86.48% functions, 52.94% lines\*\* |



\## 2. QA Objective



The purpose of this testing activity was to verify the reliability of the Woodlands API by testing:



\* API endpoint availability and response structure

\* Input validation

\* Invalid and malformed identifiers

\* HTTP error handling

\* Request-body validation

\* Authentication error handling

\* Helper-function behaviour

\* Data-related error conditions

\* Consistency of API responses

\* Regression protection through automated tests



The testing work also included identifying defects in existing API behaviour and implementing fixes where appropriate.



\## 3. Testing Scope



The API areas tested include:



\### Public API endpoints



\* `GET /api/products`

\* `GET /api/branches`

\* `GET /api/services`

\* `GET /api/testimonials`

\* `GET /api/faqs`

\* `GET /api/homepage-assets`



\### Product API



\* `GET /api/products/:id`

\* `POST /api/products`

\* `PUT /api/products/:id`

\* `DELETE /api/products/:id`



\### Branch API



\* `GET /api/branches/:id`

\* `POST /api/branches`

\* `PUT /api/branches/:id`

\* `DELETE /api/branches/:id`



\### Service API



\* `POST /api/services`

\* `PUT /api/services/:id`

\* `DELETE /api/services/:id`



\### Testimonial API



\* `POST /api/testimonials`

\* `PUT /api/testimonials/:id`

\* `DELETE /api/testimonials/:id`



\### FAQ API



\* `POST /api/faqs`

\* `PUT /api/faqs/:id`

\* `DELETE /api/faqs/:id`



\### Quote request API



\* `POST /api/quote-requests`

\* `PUT /api/quote-requests/:id`

\* `DELETE /api/quote-requests/:id`



\### Authentication



\* `POST /api/auth/login`

\* `POST /api/auth/register`



\## 4. Test Environment



Testing was performed locally using Node.js, Express, Jest, Supertest, Supabase and dotenv.



The application was modified so that the Express application can be imported by Supertest without automatically starting a listening HTTP server during tests.



The local `.env` file contains the required Supabase configuration and was not committed to GitHub.



\## 5. Test Strategy



\### 5.1 Integration/API Tests



Supertest was used to send HTTP requests to the Express application.



These tests verify:



\* HTTP status codes

\* Response bodies

\* API endpoint availability

\* Input validation

\* Invalid ID handling

\* Error responses

\* Authentication failures



\### 5.2 Unit Tests



Jest unit tests were added for:



\* `safeParse()`

\* `validateRequestBody()`



These tests verify individual helper functions independently from the API routes.



\## 6. Test Cases



\### 6.1 Basic API Health Tests



| Test                | Expected Result  | Result |

| ------------------- | ---------------- | ------ |

| GET products        | HTTP 200 + array | PASS   |

| GET branches        | HTTP 200 + array | PASS   |

| GET services        | HTTP 200 + array | PASS   |

| GET testimonials    | HTTP 200 + array | PASS   |

| GET FAQs            | HTTP 200 + array | PASS   |

| GET homepage assets | HTTP 200 + array | PASS   |



\### 6.2 Error Handling Tests



| Test                    | Expected Result    | Result |

| ----------------------- | ------------------ | ------ |

| Unknown API endpoint    | HTTP 404           | PASS   |

| Product with invalid ID | 4xx error response | PASS   |

| Branch with invalid ID  | 4xx error response | PASS   |



\### 6.3 POST Input Validation



Empty request bodies were tested against:



\* Products

\* Branches

\* Services

\* Testimonials

\* FAQs

\* Quote requests



All six tests passed.



\### 6.4 PUT Input Validation



Empty request bodies were tested against:



\* Products

\* Branches

\* Services

\* Testimonials

\* FAQs

\* Quote requests



All six tests passed.



\### 6.5 DELETE Validation



Invalid IDs were tested against:



\* Products

\* Branches

\* Services

\* Testimonials

\* FAQs

\* Quote requests



The API was verified to reject malformed IDs rather than passing invalid values directly to the database.



All six tests passed.



\## 7. Unit Test Results



\### `safeParse()`



Tested:



1\. Valid JSON array

2\. Invalid JSON

3\. Existing array

4\. `null`

5\. Empty value



All tests passed.



\### `validateRequestBody()`



Tested:



1\. Valid non-empty object

2\. Empty object

3\. Missing body

4\. Array instead of object



All tests passed.



\## 8. Authentication Testing



Authentication error handling was tested without creating permanent test users.



\### Login



Tested:



\* Invalid credentials

\* Missing credentials



Expected result:



```text

HTTP 401

```



Both tests passed.



\### Registration



Invalid registration data was submitted.



Expected result:



```text

HTTP 400

```



The test passed.



A successful registration test was deliberately not automated because the endpoint creates a real Supabase authentication user. Running such a test against the shared project database would create persistent test data.



\## 9. Defects Identified and Fixed



\### DEF-001 — Product invalid ID returned an inappropriate server error



\*\*Area:\*\* `GET /api/products/:id`



\*\*Problem:\*\* An invalid/nonexistent product ID could result in HTTP 500 rather than a meaningful not-found response.



\*\*Fix:\*\* The route now handles the Supabase `PGRST116` condition and returns HTTP 404 with:



```text

Product not found

```



Unexpected database errors continue to return HTTP 500.



\*\*QA verification:\*\* PASS



\### DEF-002 — Branch invalid ID caused a database type error



\*\*Area:\*\* `GET /api/branches/:id`



\*\*Problem:\*\* A non-numeric branch ID such as `invalid-id` could reach the database and produce a PostgreSQL type error.



\*\*Fix:\*\* The route now validates the ID before querying Supabase.



Malformed IDs return HTTP 400:



```text

Branch ID must be a valid number

```



A valid numeric ID that does not exist returns HTTP 404.



\*\*QA verification:\*\* PASS



\### DEF-003 — Empty request bodies were not consistently rejected



\*\*Area:\*\* POST and PUT endpoints



\*\*Problem:\*\* The API did not have a reusable validation mechanism to reject empty request bodies before database operations.



\*\*Fix:\*\* A reusable `validateRequestBody()` helper was introduced.



It rejects:



\* Missing bodies

\* Empty objects

\* Arrays

\* Non-object request bodies



Valid non-empty objects are accepted.



\*\*QA verification:\*\* PASS



\### DEF-004 — DELETE routes did not consistently validate IDs/not-found conditions



\*\*Area:\*\* DELETE endpoints



\*\*Problem:\*\* Invalid identifiers could reach database queries without validation, and unsuccessful deletions were not consistently reported as not-found responses.



\*\*Fix:\*\* DELETE routes now:



1\. Validate that IDs are numeric.

2\. Return HTTP 400 for malformed IDs.

3\. Perform the deletion.

4\. Check whether a record was actually deleted.

5\. Return HTTP 404 when no matching record exists.



\*\*QA verification:\*\* PASS



\## 10. Error Handling Verification



The API was tested against several classes of invalid input.



\### Invalid route



```text

GET /api/does-not-exist

```



Expected: `404`



Result: \*\*PASS\*\*



\### Invalid resource ID



```text

GET /api/products/invalid-id

GET /api/branches/invalid-id

```



Result: \*\*PASS\*\*



\### Empty request body



```text

POST /api/products

{}

```



Expected: `400`



Result: \*\*PASS\*\*



\### Invalid authentication



Invalid login credentials were tested.



Expected: `401`



Result: \*\*PASS\*\*



\## 11. Final Automated Test Results



Final Jest execution:



```text

Test Suites: 1 passed, 1 total

Tests:       39 passed, 39 total

Snapshots:   0 total

```



\*\*Final result: 39/39 automated tests passed.\*\*



No automated test failures were present in the final test run.



\## 12. Code Coverage



Final Jest coverage:



| Metric     |   Coverage |

| ---------- | ---------: |

| Statements | \*\*51.27%\*\* |

| Branches   | \*\*28.65%\*\* |

| Functions  | \*\*86.48%\*\* |

| Lines      | \*\*52.94%\*\* |



The coverage report demonstrates that automated tests execute a substantial portion of the API code, particularly reusable functions and validation-related paths.



Lower branch coverage is partly due to untested successful database mutation paths and database failure branches.



\## 13. Testing Limitations



The test suite intentionally avoids destructive or persistent operations against the shared Supabase database.



In particular:



\* Successful registration tests were not automated because they would create real authentication users.

\* Destructive successful DELETE tests were not used as routine automated tests against shared project data.

\* Full successful POST/PUT CRUD workflows would ideally use a dedicated test database or isolated fixtures.

\* Some database-specific error branches remain uncovered.

\* Coverage therefore should not be interpreted as proof that every possible database state has been tested.



A dedicated test database with seeded fixtures would allow broader end-to-end CRUD testing without risking shared project data.



\## 14. Demo Scenarios



\### Demo 1 — Invalid product ID



```text

GET /api/products/invalid-id

```



Demonstrate that the API returns an error response instead of silently accepting the invalid ID.



\### Demo 2 — Invalid branch ID



```text

GET /api/branches/invalid-id

```



Demonstrate that input validation occurs before the invalid value reaches the database.



\### Demo 3 — Empty POST body



```text

POST /api/products

{}

```



Demonstrate:



```text

400

Request body cannot be empty

```



\### Demo 4 — Invalid login



Send invalid authentication credentials.



Demonstrate:



```text

401

Invalid email or password.

```



\### Demo 5 — Automated regression test



```powershell

npm test

```



Demonstrate:



```text

39 passed, 39 total

```



\### Demo 6 — Coverage



```powershell

npm test -- --coverage

```



Show the Jest coverage summary.



\## 15. Git Evidence



Relevant QA commits:



| Commit    | Description                                |

| --------- | ------------------------------------------ |

| `b198f29` | Strengthen API response validation tests   |

| `c6df16f` | Add unit tests for validation helpers      |

| `71d8352` | Add authentication validation tests        |

| `702a6e0` | Export validation helpers for unit testing |



All QA commits were pushed to the `develop` branch.



The working tree was clean after the QA implementation was committed.



\## 16. QA Conclusion



The final automated test suite contains \*\*39 passing tests\*\* covering API availability, response validation, invalid input, error handling, helper functions and authentication validation.



Several API validation and error-handling weaknesses were identified during testing and corrected. The changes were verified through automated regression tests.



The project now has repeatable automated QA tests that can be executed with:



```powershell

npm test

```



Coverage can be generated with:



```powershell

npm test -- --coverage

```



The main remaining testing improvement would be the introduction of an isolated test database or fixture system so that successful CRUD operations can be tested end-to-end without modifying shared project data.



