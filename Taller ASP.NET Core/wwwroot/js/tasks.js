// ==================== CARGAR DETALLE DE TAREA ====================
async function loadTaskDetail(taskId) {
    try {
        const response = await fetch(`/Tasks/GetTaskDetail/${taskId}`);

        if (!response.ok) {
            throw new Error('Error al cargar el detalle de la tarea');
        }

        const html = await response.text();
        document.getElementById('task-detail-container').innerHTML = html;

        // Resaltar la tarjeta seleccionada
        document.querySelectorAll('.task-card').forEach(card => {
            card.classList.remove('selected');
        });

        const selectedCard = document.querySelector(`[data-task-id="${taskId}"]`);
        if (selectedCard) {
            selectedCard.classList.add('selected');
        }
    } catch (error) {
        console.error('Error al cargar detalle de tarea:', error);
        alert('Error al cargar los detalles de la tarea. Por favor, intenta de nuevo.');
    }
}

// ==================== DRAG & DROP CON SORTABLE.JS ====================
document.addEventListener('DOMContentLoaded', function () {
    const taskList = document.getElementById('task-list');

    if (!taskList) {
        return; // No hay lista de tareas en esta página
    }

    // Verificar que Sortable.js está disponible
    if (typeof Sortable === 'undefined') {
        console.error('Sortable.js no está cargado. Verifica que el CDN esté disponible.');
        return;
    }

    try {
        new Sortable(taskList, {
            animation: 150,
            handle: '.task-drag-handle',
            ghostClass: 'sortable-ghost',
            chosenClass: 'sortable-chosen',
            dragClass: 'sortable-drag',

            onEnd: function (evt) {
                // Solo actualizar si cambió de posición
                if (evt.oldIndex !== evt.newIndex) {
                    updateTaskOrder();
                }
            }
        });
    } catch (error) {
        console.error('Error al inicializar Sortable.js:', error);
    }
});

// ==================== ACTUALIZAR ORDEN EN EL SERVIDOR ====================
async function updateTaskOrder() {
    try {
        const taskCards = document.querySelectorAll('.task-card');
        const taskIds = Array.from(taskCards).map(card => parseInt(card.dataset.taskId));

        const response = await fetch('/Tasks/UpdateOrder', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(taskIds)
        });

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(`Error ${response.status}: ${errorText}`);
        }

        await response.json();
    } catch (error) {
        console.error('Error al actualizar el orden:', error);
        alert('Error al guardar el orden de las tareas. Por favor, intenta de nuevo.');
    }
}