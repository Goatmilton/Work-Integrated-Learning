# Woodlands Designer Boards – Mobile Application

This is a native Android Studio project created from the current Woodlands Designer Boards ASP.NET Core MVC prototype and the mobile wireframes in `TASK 1 (1).docx`.

## Current architecture

- **Android app:** Kotlin, native Android Views, local SQLite database.
- **API:** the mobile app now talks to the Woodlands API. It reads products, branches, FAQs, testimonials, users and quotes from the API and sends new quotes, contact messages and changes back to it.
- **Local database as a cache:** the local SQLite database stores a copy of the data on the phone, so the app still opens and shows content when there is no internet.
- **Offline changes:** anything created or edited while offline (for example a quote request) is saved in a waiting list on the phone and uploaded automatically once the connection is back.
- **Server address:** the app tries the hosted API first. If that can't be reached, it tries a local API on the emulator (`http://10.0.2.2:5000`). Both addresses are set in `Apiclient.kt`.
- **Website:** the original ASP.NET Core MVC website remains unchanged. The website and the app now share the same data through the API.
- **Future-ready:** the screens only talk to the local database layer and the sync layer, so the API or database behind them can be swapped later without redesigning the screens.

## Mobile functions included

- Home screen with a sliding hero banner, browse-by-category cards, featured products, the PG Bison partner banner and a testimonials strip.
- Product/service gallery with category filtering.
- Product detail pages with features, finishes, pricing and lead time.
- Product-to-quote flow, with a quote request form and confirmation.
- Branch locator with Maps intent. Admins can add and edit branches, including a branch photo.
- About Us, Testimonials, FAQs (category filters, expandable answers), Contact form.
- Legal Information: Privacy Notice, Terms of Service, Refunds and Cookies, matching the website pages.
- Connection Status screen (in the More tab) that shows whether the app can reach the Woodlands server and database, which services are online, and how many changes are still waiting to upload.
- Persistent bottom navigation: Home, Gallery, Quote, Branches, More. Staff accounts see Quotes in place of Quote.
- A genuine slide-in sidebar (opened from the header's ☰ button) with site navigation and
  account actions, separate from the More tab - matching the website's mobile hamburger menu.
- Every tappable element (buttons, cards, chips, nav items) has a ripple/darken touch reaction,
  and the active bottom-nav tab is highlighted.
- **Accounts, roles and permissions**, mirroring the website's ASP.NET Core Identity setup:
  - Register / Login / Logout. Login and registration check with the server, so an internet connection is needed for these. New accounts are created as Customers. The same seeded prototype accounts as the website are available (see `seeded accounts.md`):
    `admin@woodlandsdb.co.za` / `admin123`, `soweto@woodlandsdb.co.za` / `manager123`,
    `roodepoort@woodlandsdb.co.za` / `manager123`, `randfontein@woodlandsdb.co.za` / `manager123`,
    `customer@example.com` / `customer123`.
  - Profile screen (view role/branch, edit name/phone) and a Settings screen
    that mirrors the website's Dashboard ▸ Settings "My Account"/"Security" panels. Changing a password is not available in the app yet.
  - Role-aware Dashboard: Admin sees totals, per-branch summaries and links to manage Users,
    Products, Testimonials and FAQs; Branch Managers see their branch's pending/in-progress/
    completed counts; Customers don't get a dashboard, just My Quotes.
  - Quotes screen: Admins see every quote, Managers see their branch's quotes (with status
    controls: Pending/In Progress/Completed/Cancelled), Customers see only their own quotes
    (read-only) - the same visibility rules as `DashboardController` on the website.
  - Full CRUD management screens for Products (Admin + Managers) and, Admin-only, Users,
    Testimonials and FAQs - the same permission split as `ManagementController`/`AdminController`.
- Content comes from the API and is saved on the phone, so the catalogue, services, FAQs, testimonials, branches and quotes match what is on the website.

## Open in Android Studio

1. Open the `WoodlandsMobile` folder in Android Studio.
2. Allow Android Studio to sync Gradle and install any requested Android SDK components.
3. Use an Android emulator or a physical Android device running Android 15 (API 35) or newer.
4. Run the `app` configuration.

The app needs an internet connection the first time it opens, so it can download the catalogue. After that it can be used offline.

The generated `build/`, `.gradle/` and `.idea/` folders are not needed to open the project and can be left out when sharing it.

## Important prototype behaviour

Quote and contact submissions are sent to the Woodlands API, so staff on the website and in the app can see them. If the phone is offline, the submission is kept on the phone and sent later, and the Connection Status screen shows how many changes are still waiting.

Signing in, registering and the first download of data all need a connection. Browsing content that has already been downloaded works offline.

The source website contains several Unsplash image URLs. To keep this Android prototype usable without requiring a network image service, the app uses the supplied local Woodlands product photographs as packaged fallback imagery for those catalogue entries. Images that come from the API are downloaded once and kept on the phone.

The app allows plain `http` traffic so it can reach the local API on the emulator. This should be switched off before a real release.

## Where to edit the design

The code is split by concern so no single file gets unwieldy:

- `MainActivity.kt` - app chrome (header, bottom nav), screen switching, the gallery, product, quote, More and contact screens, and every
  shared UI helper (`button`, `card`, `chip`, `field`, colours, ripple/touch-reaction backgrounds).
- `Homescreens.kt` - the home screen sections (hero banner, categories, featured products, PG Bison banner, testimonials strip, footer).
- `Infoscreens.kt` - About Us, Testimonials, FAQs and the Legal Information screens.
- `BranchScreens.kt` - branch list, branch details and the admin branch form.
- `ConnectionsCard.kt` - the Connection Status screen.
- `Sidebar.kt` - the slide-in navigation drawer opened from the header's ☰ button.
- `AuthScreens.kt` - Login, Register, Profile, Settings.
- `AdminScreens.kt` - Dashboard, Quotes, and the Users/Products/Testimonials/FAQs management
  screens, gated by role.
- `Apiclient.kt` - sends requests to the API (hosted first, then local) and reports success, failure or offline.
- `Syncmanager.kt` - downloads data from the API, uploads waiting changes, handles login/register and the connection checks.
- `Woodlandsapp.kt` - starts the sync when the app opens and whenever the network comes back.
- `ImageLoader.kt` - downloads and caches images into the database.
- `LocalDb.kt` - SQLite tables for the on-phone copy of the data and the waiting list of changes.
- `Queries.kt` - all read/write helpers against `LocalDb`, used by every screen file.
- `Models.kt` - data classes plus the `Roles` object mirroring `IdentitySeederRoles` on the website.
- `Session.kt` - the signed-in-user session. `Security.kt` is an older local password helper and is not used for login any more.

Colours, typography and layout constants are intentionally straightforward so the design can be changed quickly.

The packaged product images are in:

`app/src/main/res/drawable-nodpi/`

## Moving to a production setup

The app already follows this path:

`Android UI -> Local database and sync -> REST API -> ASP.NET Core services -> EF Core -> database`

When the final hosting is chosen, only the server addresses in `Apiclient.kt` need to change. Do not make the Android app connect directly to the production SQL database.
