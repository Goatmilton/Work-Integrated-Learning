const request = require("supertest");
const app = require("../server");

describe("API Health / Basic Endpoint Tests", () => {

    test("GET /api/products should return a response", async () => {
        const response = await request(app)
            .get("/api/products");

        expect(response.status).toBeLessThan(500);
    });

    test("GET /api/branches should return a response", async () => {
        const response = await request(app)
            .get("/api/branches");

        expect(response.status).toBeLessThan(500);
    });

    test("GET /api/services should return a response", async () => {
        const response = await request(app)
            .get("/api/services");

        expect(response.status).toBeLessThan(500);
    });

    test("GET /api/testimonials should return a response", async () => {
        const response = await request(app)
            .get("/api/testimonials");

        expect(response.status).toBeLessThan(500);
    });

    test("GET /api/faqs should return a response", async () => {
        const response = await request(app)
            .get("/api/faqs");

        expect(response.status).toBeLessThan(500);
    });

    test("GET /api/homepage-assets should return a response", async () => {
        const response = await request(app)
            .get("/api/homepage-assets");

        expect(response.status).toBeLessThan(500);
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
});