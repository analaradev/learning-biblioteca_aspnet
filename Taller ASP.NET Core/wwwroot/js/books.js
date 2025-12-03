// ==================== CARGAR DETALLE DE LIBRO ====================
async function loadBookDetail(bookId) {
    try {
        const response = await fetch(`/Books/GetBookDetail/${bookId}`);

        if (!response.ok) {
            throw new Error('Error al cargar el detalle del libro');
        }

        const html = await response.text();
        document.getElementById('book-detail-container').innerHTML = html;

        // Resaltar la tarjeta seleccionada
        document.querySelectorAll('.book-card').forEach(card => {
            card.classList.remove('selected');
        });

        const selectedCard = document.querySelector(`[data-book-id="${bookId}"]`);
        if (selectedCard) {
            selectedCard.classList.add('selected');
        }
    } catch (error) {
        console.error('Error al cargar detalle de libro:', error);
        alert('Error al cargar los detalles del libro. Por favor, intenta de nuevo.');
    }
}

// ==================== DRAG & DROP CON SORTABLE.JS ====================
document.addEventListener('DOMContentLoaded', function () {
    const bookList = document.getElementById('book-list');

    if (!bookList) {
        return; // No hay lista de libros en esta página
    }

    // Verificar que Sortable.js está disponible
    if (typeof Sortable === 'undefined') {
        console.error('Sortable.js no está cargado. Verifica que el CDN esté disponible.');
        return;
    }

    try {
        new Sortable(bookList, {
            animation: 150,
            handle: '.book-drag-handle',
            ghostClass: 'sortable-ghost',
            chosenClass: 'sortable-chosen',
            dragClass: 'sortable-drag',

            onEnd: function (evt) {
                // Solo actualizar si cambió de posición
                if (evt.oldIndex !== evt.newIndex) {
                    updateBookOrder();
                }
            }
        });
    } catch (error) {
        console.error('Error al inicializar Sortable.js:', error);
    }
});

// ==================== ACTUALIZAR ORDEN EN EL SERVIDOR ====================
async function updateBookOrder() {
    try {
        const bookCards = document.querySelectorAll('.book-card');
        const bookIds = Array.from(bookCards).map(card => parseInt(card.dataset.bookId));

        const response = await fetch('/Books/UpdateOrder', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(bookIds)
        });

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(`Error ${response.status}: ${errorText}`);
        }

        await response.json();
    } catch (error) {
        console.error('Error al actualizar el orden:', error);
        alert('Error al guardar el orden de los libros. Por favor, intenta de nuevo.');
    }
}