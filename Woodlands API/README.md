# Woodlands API local setup

1. Copy `.env.example` to `.env` and set `SUPABASE_URL`, `SUPABASE_KEY`, and `API_INTERNAL_KEY`.
2. Keep `.env` private; never commit it. `SUPABASE_KEY` must be the server-side service-role key and must not be exposed to browsers.
3. Start the API with `npm install` and `npm start`.
4. Configure the ASP.NET app's `API_INTERNAL_KEY` environment variable to the same value as the API's `API_INTERNAL_KEY`. The ASP.NET app sends it only on server-to-server API requests.
5. Set `NodeApi__BaseUrl` on the ASP.NET app if the API is not at `http://localhost:5000/`.

Do not seed production until the target Supabase project and schema have been confirmed.
