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

});