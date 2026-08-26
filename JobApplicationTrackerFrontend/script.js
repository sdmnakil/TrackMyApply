// ==============================
// CONFIG
// ==============================
const isLocal = window.location.hostname === "localhost" || window.location.hostname === "127.0.0.1";

const API_BASE_URL = isLocal 
    ? "http://localhost:5189/api" 
    : "https://railway.app"; // <-- Your clean direct Railway link




// ==============================
// STATE (current filters/pagination mone rakhar jonno)
// ==============================
let currentPage = 1;
const pageSize = 10;
let totalPages = 1;
let editingId = null; // null hole "Add mode", value thakle "Edit mode"

// ==============================
// HELPER FUNCTIONS
// ==============================
function saveToken(token) {
    localStorage.setItem("token", token);
}

function getToken() {
    return localStorage.getItem("token");
}

function logout() {
    localStorage.removeItem("token");
    window.location.href = "login.html";
}

function requireAuth() {
    if (!getToken()) {
        window.location.href = "login.html";
    }
}

// ==============================
// REGISTER PAGE LOGIC
// ==============================
async function handleRegister(event) {
    event.preventDefault();

    const username = document.getElementById("username").value;
    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;
    const errorBox = document.getElementById("errorBox");

    errorBox.textContent = "";

    try {
        const response = await fetch(`${API_BASE_URL}/Auth/register`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ username, email, password })
        });

        if (!response.ok) {
            const errData = await response.json().catch(() => null);
            errorBox.textContent = errData?.message || "Registration failed.";
            return;
        }

        alert("Registration successful! Please login.");
        window.location.href = "login.html";
    } catch (err) {
        errorBox.textContent = "Server-er sathe connect kora jacche na.";
    }
}

// ==============================
// LOGIN PAGE LOGIC
// ==============================
async function handleLogin(event) {
    event.preventDefault();

    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;
    const errorBox = document.getElementById("errorBox");

    errorBox.textContent = "";

    try {
        const response = await fetch(`${API_BASE_URL}/Auth/login`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ email, password })
        });

        if (!response.ok) {
            errorBox.textContent = "Invalid email or password.";
            return;
        }

        const data = await response.json();
        saveToken(data.token);
        window.location.href = "dashboard.html";
    } catch (err) {
        errorBox.textContent = "Server-er sathe connect kora jacche na.";
    }
}

// ==============================
// DASHBOARD: STATS
// ==============================
async function loadDashboardStats() {
    const response = await fetch(`${API_BASE_URL}/Dashboard`, {
        headers: { "Authorization": `Bearer ${getToken()}` }
    });

    if (response.status === 401) {
        logout();
        return;
    }

    const data = await response.json();

    document.getElementById("statTotal").textContent = data.totalApplications ?? 0;
    document.getElementById("statApplied").textContent = data.applied ?? 0;
    document.getElementById("statInterview").textContent = data.interview ?? 0;
    document.getElementById("statOffer").textContent = data.offer ?? 0;
    document.getElementById("statRejected").textContent = data.rejected ?? 0;
}

// ==============================
// DASHBOARD: LOAD LIST (Pagination + Search + Sort shob ekhane handle)
// ==============================
async function loadApplications() {
    const company = document.getElementById("filterCompany").value.trim();
    const status = document.getElementById("filterStatus").value;
    const sortBy = document.getElementById("sortBy").value;
    const sortOrder = document.getElementById("sortOrder").value;

    const isFiltering = company !== "" || status !== "";

    let url;
    if (isFiltering) {
        // Search endpoint use hobe. Note: backend-er search endpoint
        // pagination support kore na, tai shob matching result ekbare ashbe.
        const params = new URLSearchParams();
        if (company) params.append("company", company);
        if (status) params.append("status", status);
        params.append("sortBy", sortBy);
        params.append("sortOrder", sortOrder);

        url = `${API_BASE_URL}/JobApplications/search?${params.toString()}`;
    } else {
        // Normal GetAll endpoint, jeta pagination support kore.
        const params = new URLSearchParams({
            page: currentPage,
            pageSize: pageSize,
            sortBy: sortBy,
            sortOrder: sortOrder
        });

        url = `${API_BASE_URL}/JobApplications?${params.toString()}`;
    }

    const response = await fetch(url, {
        headers: { "Authorization": `Bearer ${getToken()}` }
    });

    if (response.status === 401) {
        logout();
        return;
    }

    const data = await response.json();

    if (isFiltering) {
        // Search endpoint shudhu ekta plain array return kore.
        renderTable(data);
        document.getElementById("paginationBar").style.display = "none";
    } else {
        // GetAll endpoint PagedResultDto (items, totalPages, etc.) return kore.
        renderTable(data.items);
        totalPages = data.totalPages || 1;
        document.getElementById("pageInfo").textContent =
            `Page ${data.page} of ${totalPages} (Total: ${data.totalCount})`;
        document.getElementById("paginationBar").style.display = "flex";
    }
}

function applyFilters() {
    currentPage = 1;
    loadApplications();
}

function resetFilters() {
    document.getElementById("filterCompany").value = "";
    document.getElementById("filterStatus").value = "";
    document.getElementById("sortBy").value = "applicationDate";
    document.getElementById("sortOrder").value = "desc";
    currentPage = 1;
    loadApplications();
}

function goToNextPage() {
    if (currentPage < totalPages) {
        currentPage++;
        loadApplications();
    }
}

function goToPreviousPage() {
    if (currentPage > 1) {
        currentPage--;
        loadApplications();
    }
}

// ==============================
// DASHBOARD: RENDER TABLE
// ==============================
function renderTable(applications) {
    const tbody = document.getElementById("jobTableBody");
    tbody.innerHTML = "";

    if (!applications || applications.length === 0) {
        tbody.innerHTML = `<tr><td colspan="7" style="text-align:center;">Kono application paoa jayni.</td></tr>`;
        return;
    }

    applications.forEach(app => {
        const row = document.createElement("tr");
        row.innerHTML = `
            <td>${app.companyName}</td>
            <td>${app.jobTitle}</td>
            <td>${app.jobType}</td>
            <td>${app.location}</td>
            <td>${new Date(app.applicationDate).toLocaleDateString()}</td>
            <td>${app.status}</td>
            <td>
                <button onclick='startEdit(${JSON.stringify(app)})'>Edit</button>
                <button class="danger" onclick="deleteApplication(${app.id})">Delete</button>
            </td>
        `;
        tbody.appendChild(row);
    });
}

// ==============================
// DASHBOARD: ADD / EDIT (shared form)
// ==============================
function startEdit(app) {
    editingId = app.id;

    document.getElementById("companyName").value = app.companyName;
    document.getElementById("jobTitle").value = app.jobTitle;
    document.getElementById("jobType").value = app.jobType;
    document.getElementById("location").value = app.location;
    document.getElementById("salaryMin").value = app.salaryMin ?? "";
    document.getElementById("salaryMax").value = app.salaryMax ?? "";
    document.getElementById("isSalaryNegotiable").checked = app.isSalaryNegotiable;
    document.getElementById("applicationDate").value = app.applicationDate.split("T")[0];
    document.getElementById("status").value = app.status;
    document.getElementById("jobUrl").value = app.jobUrl ?? "";
    document.getElementById("notes").value = app.notes ?? "";

    document.getElementById("formTitle").textContent = "Edit Application";
    document.getElementById("submitBtn").textContent = "Update Application";
    document.getElementById("cancelEditBtn").style.display = "block";

    window.scrollTo({ top: 0, behavior: "smooth" });
}

function cancelEdit() {
    editingId = null;
    document.getElementById("addForm").reset();
    document.getElementById("formTitle").textContent = "Add New Application";
    document.getElementById("submitBtn").textContent = "Add Application";
    document.getElementById("cancelEditBtn").style.display = "none";
}

function buildPayloadFromForm() {
    return {
        companyName: document.getElementById("companyName").value,
        jobTitle: document.getElementById("jobTitle").value,
        jobType: document.getElementById("jobType").value,
        location: document.getElementById("location").value,
        salaryMin: document.getElementById("salaryMin").value || null,
        salaryMax: document.getElementById("salaryMax").value || null,
        isSalaryNegotiable: document.getElementById("isSalaryNegotiable").checked,
        applicationDate: document.getElementById("applicationDate").value,
        status: document.getElementById("status").value,
        jobUrl: document.getElementById("jobUrl").value,
        notes: document.getElementById("notes").value
    };
}

async function handleFormSubmit(event) {
    event.preventDefault();

    const payload = buildPayloadFromForm();
    const errorBox = document.getElementById("addErrorBox");
    errorBox.textContent = "";

    try {
        let response;

        if (editingId) {
            // Update mode
            response = await fetch(`${API_BASE_URL}/JobApplications/${editingId}`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${getToken()}`
                },
                body: JSON.stringify(payload)
            });
        } else {
            // Add mode
            response = await fetch(`${API_BASE_URL}/JobApplications`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${getToken()}`
                },
                body: JSON.stringify(payload)
            });
        }

        if (!response.ok) {
            const errData = await response.json().catch(() => null);
            errorBox.textContent = errData?.message || errData?.title || "Kaj ta kora jayni. Shob field thik moto fill koro.";
            return;
        }

        cancelEdit();
        loadApplications();
        loadDashboardStats();
    } catch (err) {
        errorBox.textContent = "Server-er sathe connect kora jacche na.";
    }
}

async function deleteApplication(id) {
    if (!confirm("Ei application delete korte chao?")) return;

    const response = await fetch(`${API_BASE_URL}/JobApplications/${id}`, {
        method: "DELETE",
        headers: { "Authorization": `Bearer ${getToken()}` }
    });

    if (response.status === 401) {
        logout();
        return;
    }

    loadApplications();
    loadDashboardStats();
}
