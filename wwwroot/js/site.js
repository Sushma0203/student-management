document.addEventListener("DOMContentLoaded", function () {

    setTimeout(() => {
        document.querySelectorAll('.alert').forEach(alert => alert.remove());
    }, 3000);

});

function searchTable() {
    let input = document.getElementById("searchInput").value.toLowerCase();
    let rows = document.querySelectorAll("tbody tr");

    rows.forEach(r => {
        r.style.display = r.innerText.toLowerCase().includes(input) ? "" : "none";
    });
}
