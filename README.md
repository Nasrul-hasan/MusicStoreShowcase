# Music Store Showcase

##  Overview
Music Store Showcase is a single-page ASP.NET Core MVC application that generates and displays procedurally created music data.  
The application demonstrates deterministic data generation using seeds, localization support, pagination, infinite scrolling, and media generation (cover images and audio previews).

---

##  Features

### 🎛️ Toolbar Controls
- Region selection (en-US, de-DE)
- Seed input (deterministic data generation)
- Random seed generator
- Likes control (supports fractional values like 0.5, 3.5, etc.)
- View switch (Table / Gallery)

---

### 📊 Table View
- Paginated song list
- Displays:
  - Index
  - Title
  - Artist
  - Album
  - Genre
  - Likes
- Click on a row to expand details:
  - Cover image
  - Generated review text
  - Playable audio preview

---

### 🖼️ Gallery View
- Card-based layout
- Infinite scrolling (auto-load more data)
- Displays:
  - Cover image
  - Title
  - Artist
  - Album
  - Genre
  - Likes

---

### Deterministic Data Generation
- All data is generated based on:
  - Region
  - Seed
  - Page
  - Item index
- Same seed → same data
- Changing seed → different data
- Changing likes → only likes change

---

### 🌍 Localization
- Supports multiple regions:
  - English (US)
  - German (DE)
- Locale-specific datasets are loaded from JSON files
- Titles, artists, albums, and reviews vary per region

---

### ❤️ Likes System
- Integer and fractional input supported
- Example:
  - 0 → all songs have 0 likes
  - 0.5 → mostly 0, some 1
  - 5 → around 5 likes
- Likes are generated independently using seeded randomness

---

### 🖼️ Cover Generation
- Covers are dynamically generated using:
  - A pool of image assets
  - Seed-based selection
  - Text overlays (title & artist)
- Ensures:
  - High visual variety
  - Deterministic output
  - Realistic and appealing covers

---

### 🔊 Audio Preview
- Each song has a generated audio preview
- WAV format (browser playable)
- Deterministic (same seed → same audio)
- Generated using simple note synthesis

---

## 🧠 Technical Details

### Backend
- ASP.NET Core MVC
- REST-style endpoints:
  - `/songs/list`
  - `/songs/details`
  - `/songs/cover`
  - `/songs/preview`

### Frontend
- Vanilla JavaScript (no frameworks)
- Fetch API
- Dynamic DOM rendering

### Data Generation
- Seed-based random generation
- Stable hashing for reproducibility

---

## ▶️ Running the Project

1. Open in Visual Studio
2. Build the solution
3. Run the project
4. Open in browser

---

## 🌐 Deployment

The project is deployed and accessible here:

👉 ** **

---

## 🎥 Demo Video

Video demonstration:

👉 ** **

---


