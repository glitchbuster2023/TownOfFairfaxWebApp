export function showElement(id) {
    document.getElementById(id).style.display = 'block';
}

export function hideElement(id) {
    document.getElementById(id).style.display = 'none';
}

export function changeElementText(id, text) {
    document.getElementById(id).textContent = text;
}