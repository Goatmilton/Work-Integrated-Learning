const request = require("supertest");
const app = require("../server");

describe("API Health / Basic Endpoint Tests", () => {

    test("GET /api/products should return 200 and an array", async () => {
        const response = await request(app)
            .get("/api/products");

        expect(response.status).toBe(200);
        expect(Array.isArray(response.body)).toBe(true);
    });

    test("GET /api/branches should return 200 and an array", async () => {
        const response = await request(app)
            .get("/api/branches");

        expect(response.status).toBe(200);
        expect(Array.isArray(response.body)).toBe(true);
    });

    test("GET /api/services should return 200 and an array", async () => {
        const response = await request(app)
            .get("/api/services");

        expect(response.status).toBe(200);
        expect(Array.isArray(response.body)).toBe(true);
    });

    test("GET /api/testimonials should return 200 and an array", async () => {
        const response = await request(app)
            .get("/api/testimonials");

        expect(response.status).toBe(200);
        expect(Array.isArray(response.body)).toBe(true);
    });

    test("GET /api/faqs should return 200 and an array", async () => {
        const response = await request(app)
            .get("/api/faqs");

        expect(response.status).toBe(200);
        expect(Array.isArray(response.body)).toBe(true);
    });

    test("GET /api/homepage-assets should return 200 and an array", async () => {
        const response = await request(app)
            .get("/api/homepage-assets");

        expect(response.status).toBe(200);
        expect(Array.isArray(response.body)).toBe(true);
    });

});

describe("API Error Handling Tests", () => {

    test("GET unknown endpoint should return 404", async () => {
        const response = await request(app)
            .get("/api/does-not-exist");

        expect(response.status).toBe(404);
    });

    test("GET product with invalid ID should return an error response", async () => {
        const response = await request(app)
            .get("/api/products/invalid-id");

        expect(response.status).toBeGreaterThanOrEqual(400);
        expect(response.status).toBeLessThan(500);
    });

    test("GET branch with invalid ID should return an error response", async () => {
        const response = await request(app)
            .get("/api/branches/invalid-id");

        expect(response.status).toBeGreaterThanOrEqual(400);
        expect(response.status).toBeLessThan(500);
    });

});
describe("API Input Validation Tests", () => {

    test("POST /api/products with empty body should return an error", async () => {
        const response = await request(app)
            .post("/api/products")
            .send({});

        expect(response.status).toBeGreaterThanOrEqual(400);
        expect(response.status).toBeLessThan(500);
    });

    test("POST /api/branches with empty body should return an error", async () => {
        const response = await request(app)
            .post("/api/branches")
            .send({});

        expect(response.status).toBeGreaterThanOrEqual(400);
        expect(response.status).toBeLessThan(500);
    });

    test("POST /api/services with empty body should return an error", async () => {
        const response = await request(app)
            .post("/api/services")
            .send({});

        expect(response.status).toBeGreaterThanOrEqual(400);
        expect(response.status).toBeLessThan(500);
    });

    test("POST /api/testimonials with empty body should return an error", async () => {
        const response = await request(app)
            .post("/api/testimonials")
            .send({});

        expect(response.status).toBeGreaterThanOrEqual(400);
        expect(response.status).toBeLessThan(500);
    });

    test("POST /api/faqs with empty body should return an error", async () => {
        const response = await request(app)
            .post("/api/faqs")
            .send({});

        expect(response.status).toBeGreaterThanOrEqual(400);
        expect(response.status).toBeLessThan(500);
    });

    test("POST /api/quote-requests with empty body should return an error", async () => {
        const response = await request(app)
            .post("/api/quote-requests")
            .send({});

        expect(response.status).toBeGreaterThanOrEqual(400);
        expect(response.status).toBeLessThan(500);
    });

});

describe("API Update Validation Tests", () => {

    test("PUT /api/products/:id with empty body should return an error", async () => {
        const response = await request(app)
            .put("/api/products/999999")
            .send({});

        expect(response.status).toBeGreaterThanOrEqual(400);
        expect(response.status).toBeLessThan(500);
    });

    test("PUT /api/branches/:id with empty body should return an error", async () => {
        const response = await request(app)
            .put("/api/branches/999999")
            .send({});

        expect(response.status).toBeGreaterThanOrEqual(400);
        expect(response.status).toBeLessThan(500);
    });

    test("PUT /api/services/:id with empty body should return an error", async () => {
        const response = await request(app)
            .put("/api/services/999999")
            .send({});

        expect(response.status).toBeGreaterThanOrEqual(400);
        expect(response.status).toBeLessThan(500);
    });

    test("PUT /api/testimonials/:id with empty body should return an error", async () => {
        const response = await request(app)
            .put("/api/testimonials/999999")
            .send({});

        expect(response.status).toBeGreaterThanOrEqual(400);
        expect(response.status).toBeLessThan(500);
    });

    test("PUT /api/faqs/:id with empty body should return an error", async () => {
        const response = await request(app)
            .put("/api/faqs/999999")
            .send({});

        expect(response.status).toBeGreaterThanOrEqual(400);
        expect(response.status).toBeLessThan(500);
    });

    test("PUT /api/quote-requests/:id with empty body should return an error", async () => {
        const response = await request(app)
            .put("/api/quote-requests/999999")
            .send({});

        expect(response.status).toBeGreaterThanOrEqual(400);
        expect(response.status).toBeLessThan(500);
    });

});
describe("API Delete Validation Tests", () => {

    test("DELETE /api/products/:id with invalid ID should return an error", async () => {
        const response = await request(app)
            .delete("/api/products/invalid-id");

        expect(response.status).toBeGreaterThanOrEqual(400);
        expect(response.status).toBeLessThan(500);
    });

    test("DELETE /api/branches/:id with invalid ID should return an error", async () => {
        const response = await request(app)
            .delete("/api/branches/invalid-id");

        expect(response.status).toBeGreaterThanOrEqual(400);
        expect(response.status).toBeLessThan(500);
    });

    test("DELETE /api/services/:id with invalid ID should return an error", async () => {
        const response = await request(app)
            .delete("/api/services/invalid-id");

        expect(response.status).toBeGreaterThanOrEqual(400);
        expect(response.status).toBeLessThan(500);
    });

    test("DELETE /api/testimonials/:id with invalid ID should return an error", async () => {
        const response = await request(app)
            .delete("/api/testimonials/invalid-id");

        expect(response.status).toBeGreaterThanOrEqual(400);
        expect(response.status).toBeLessThan(500);
    });

    test("DELETE /api/faqs/:id with invalid ID should return an error", async () => {
        const response = await request(app)
            .delete("/api/faqs/invalid-id");

        expect(response.status).toBeGreaterThanOrEqual(400);
        expect(response.status).toBeLessThan(500);
    });

    test("DELETE /api/quote-requests/:id with invalid ID should return an error", async () => {
        const response = await request(app)
            .delete("/api/quote-requests/invalid-id");

        expect(response.status).toBeGreaterThanOrEqual(400);
        expect(response.status).toBeLessThan(500);
    });

});
