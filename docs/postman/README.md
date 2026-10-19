# FurniSpace API - Postman

## Import

1. Buka Postman dan pilih **Import**.
2. Import `FurniSpace.postman_collection.json`.
3. Import `FurniSpace.postman_environment.json`.
4. Pilih environment **FurniSpace Local** di kanan atas.

## Jalankan API

Dari root backend:

```powershell
dotnet run --project src/FurniSpace.Api --launch-profile http
```

Default URL dari launch profile adalah `http://localhost:5264`. Jika port berubah,
ubah nilai `baseUrl` pada environment Postman.

## Alur pengujian

Jalankan request dalam urutan berikut:

1. `System / Health Check`
2. `System / Database Health Check`
3. `Authentication / Register Customer`
4. `Authentication / Login`
5. `Authentication / Current User`
6. `Authentication / Refresh Token`
7. `Authentication / Current User` sekali lagi untuk menguji access token baru
8. `Authentication / Logout`

Script test pada Register, Login, dan Refresh Token otomatis menyimpan
`accessToken`, `refreshToken`, dan `userId` ke environment aktif. Logout otomatis
menghapus token tersebut.

Jika email pada contoh sudah pernah didaftarkan, Register akan menghasilkan HTTP
409. Anda dapat langsung menjalankan Login atau mengganti `customerEmail` pada
environment.
