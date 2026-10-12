# Login / menu migration

Only login and navigation were migrated from `mvc_Dapper6B`. Other business controllers and views remain in that source directory and are not part of the main application. Their menu destinations will not work until those features are migrated.

## Run

Run `dotnet run --project mvcStore.csproj`, then open the launch URL. `/` opens the login page. The database must provide the original `z_org_user` and `z_sys_program` tables. `ConnectionStrings:mvcstore` was copied into the main appsettings.json; do not commit database credentials to a public repository.

Vue 3 is mounted on the login, header and sidebar independently. Existing MVC content stays server-rendered. No Vite, Node server, Vue Router or additional NuGet packages are needed at runtime. The installed Vue production file and license were copied into `wwwroot/lib/vue`; after upgrading Vue, copy those files again.

AdminLTE 4.0.0-rc7 CSS/JS, logo, login images and relevant styles are local. Icon fonts and Source Sans use the same external CDN URLs as the source application.

Login retains the source database credential query and enabled-user check. Menu filtering retains `is_enabled = 1 AND code_no = current user code`, ordering, two-level groups and query-string links. Menu visibility is not a substitute for authorization on future business endpoints.

Authentication now uses protected ASP.NET Core cookies; remember-me lasts 30 days. The old unprotected A/B/C cookies are not used. Login and logout POSTs require antiforgery tokens. Keep ASP.NET Core data-protection keys persistent when deploying so remembered logins survive restarts.

## Independence verification

The project excludes `*mvc_Dapper*/**`, `node_modules/**` and `.verification/**` from SDK items. Reference folder names such as `mvc_Dapper6B` and `xxx_mvc_Dapper6B` are both excluded from compilation and publishing. The reference folder can be deleted without affecting the main application; if renaming it, keep `mvc_Dapper` in its name so this exclusion continues to apply. All runtime assets and code required by this migration are in the main project. A standalone copy under `.verification/standalone` without the source directory or node_modules was compiled and started successfully.

Verified: main and standalone builds have zero warnings/errors; JavaScript syntax; login/static assets return HTTP 200; unauthenticated Home redirects; menu API returns 401; POST without antiforgery returns 400; empty credentials return 400.

Browser visual verification and successful login with an actual account still require a local browser and valid credentials. The original source directory has not been deleted.
