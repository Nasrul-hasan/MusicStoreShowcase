let currentPage = 1;
const pageSize = 20;
let currentView = "table";
let isLoading = false;
let expandedSongId = null;

async function loadSongs() {
    document.getElementById('loading').style.display = 'block';

    const region = document.getElementById('region').value;
    const seed = document.getElementById('seed').value;
    const likes = document.getElementById('likes').value;

    const url = `/songs/list?region=${region}&seed=${seed}&likes=${likes}&page=${currentPage}&pageSize=${pageSize}`;

    const response = await fetch(url);
    const songs = await response.json();

    if (currentView === "table") {
        renderTable(songs);
    } else {
        renderGallery(songs, currentPage === 1);
    }

    const pageNumber = document.getElementById('pageNumber');
    if (pageNumber) {
        pageNumber.innerText = currentPage;
    }

    document.getElementById('loading').style.display = 'none';
}

function renderTable(songs) {
    document.getElementById('tableContainer').style.display = 'block';
    document.getElementById('galleryContainer').style.display = 'none';

    document.getElementById('prevBtn').style.display = 'inline-block';
    document.getElementById('nextBtn').style.display = 'inline-block';

    const tableBody = document.getElementById('songTableBody');
    tableBody.innerHTML = '';

    songs.forEach(song => {
        const row = document.createElement('tr');
        row.style.cursor = 'pointer';

        row.innerHTML = `
            <td>${song.index}</td>
            <td>${song.title}</td>
            <td>${song.artist}</td>
            <td>${song.album}</td>
            <td>${song.genre}</td>
            <td>${song.likes}</td>
        `;

        row.addEventListener('click', async () => {
            await toggleDetails(song, row);
        });

        tableBody.appendChild(row);
    });

    expandedSongId = null;
    isLoading = false;
}

function renderGallery(songs, isFirstPage) {
    document.getElementById('tableContainer').style.display = 'none';
    document.getElementById('galleryContainer').style.display = 'block';

    document.getElementById('prevBtn').style.display = 'none';
    document.getElementById('nextBtn').style.display = 'none';

    const grid = document.getElementById('galleryGrid');

    if (isFirstPage) {
        grid.innerHTML = '';
    }

    songs.forEach(song => {
        const card = document.createElement('div');
        card.className = 'gallery-card';
        card.style.cursor = 'pointer';

        card.innerHTML = `
            <img src="/songs/cover?id=${encodeURIComponent(song.id)}&title=${encodeURIComponent(song.title)}&artist=${encodeURIComponent(song.artist)}"
                 alt="Cover"
                 style="width:100%; height:180px; object-fit:cover; margin-bottom:10px;" />
            <strong>${song.title}</strong><br/>
            <small>${song.artist}</small><br/>
            <small>${song.album}</small><br/>
            <small>${song.genre}</small><br/>
            <small>❤️ ${song.likes}</small>
        `;

        grid.appendChild(card);
    });

    isLoading = false;
}

function resetAndLoad() {
    currentPage = 1;
    expandedSongId = null;

    if (currentView === "gallery") {
        window.scrollTo(0, 0);
    }

    loadSongs();
}

function generateRandomSeed() {
    const randomSeed = Math.floor(Math.random() * 9000000000);
    document.getElementById('seed').value = randomSeed;
    resetAndLoad();
}

async function toggleDetails(song, clickedRow) {
    const existingDetailsRow = document.getElementById('detailsRow');

    if (existingDetailsRow && expandedSongId === song.id) {
        existingDetailsRow.remove();
        expandedSongId = null;
        return;
    }

    if (existingDetailsRow) {
        existingDetailsRow.remove();
    }

    const region = document.getElementById('region').value;

    const response = await fetch(
        `/songs/details?id=${encodeURIComponent(song.id)}&title=${encodeURIComponent(song.title)}&artist=${encodeURIComponent(song.artist)}&region=${encodeURIComponent(region)}`
    );
    const details = await response.json();

    const detailsRow = document.createElement('tr');
    detailsRow.id = 'detailsRow';

    detailsRow.innerHTML = `
    <td colspan="6">
        <div class="details-panel">
            <div class="details-content">
                <img src="${details.coverUrl}" alt="Cover" width="180" height="180" />
                <div class="details-text">
                    <p><strong>Review:</strong> ${details.review}</p>
                    <p><strong>Preview:</strong></p>
                    <audio controls src="${details.previewUrl}"></audio>
                </div>
            </div>
        </div>
    </td>
`;

    clickedRow.insertAdjacentElement('afterend', detailsRow);
    expandedSongId = song.id;
}

document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('region').addEventListener('change', resetAndLoad);
    document.getElementById('seed').addEventListener('input', resetAndLoad);
    document.getElementById('likes').addEventListener('input', resetAndLoad);

    window.addEventListener('scroll', () => {
        if (currentView !== "gallery") return;
        if (isLoading) return;

        const scrollTop = window.scrollY;
        const windowHeight = window.innerHeight;
        const fullHeight = document.body.offsetHeight;

        if (scrollTop + windowHeight >= fullHeight - 100) {
            isLoading = true;
            currentPage++;
            loadSongs();
        }
    });

    document.getElementById('viewMode').addEventListener('change', (e) => {
        currentView = e.target.value;
        resetAndLoad();
    });

    document.getElementById('prevBtn').addEventListener('click', () => {
        if (currentPage > 1) {
            currentPage--;
            loadSongs();
        }
    });

    document.getElementById('nextBtn').addEventListener('click', () => {
        currentPage++;
        loadSongs();
    });

    document.getElementById('randomSeedBtn').addEventListener('click', generateRandomSeed);

    loadSongs();
});