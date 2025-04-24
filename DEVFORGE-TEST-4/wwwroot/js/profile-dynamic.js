document.addEventListener("DOMContentLoaded", function () {
    const carousel = document.getElementById("userCarousel");
    if (!carousel) return;

    // Función que carga los datos de perfil
    const loadProfile = (userId) => {
        if (!userId) return;

        console.log("User ID activo:", userId);

        fetch(`/api/UserProfile/${userId}`)
            .then(response => response.json())
            .then(data => {
                // Actualiza texto del perfil
                document.querySelector("[data-profile='role']").textContent = data.roleVisible ?? "Sin rol visible";
                document.querySelector("[data-profile='bio']").textContent = data.bio ?? "";
                document.querySelector("[data-profile='skills']").innerHTML =
                    data.skills?.split(',').map(skill =>
                        `<span class="badge bg-primary bg-gradient rounded-pill px-3 py-2">${skill.trim()}</span>`
                    ).join('') ?? "";

                // Contacto
                document.querySelector("[data-profile='email']").textContent = data.email ?? "";
                document.querySelector("[data-profile='email']").href = `mailto:${data.email ?? ""}`;
                document.querySelector("[data-profile='phone']").textContent = data.phone ?? "";

                // Redes
                const github = document.querySelector("[data-profile='github']");
                const twitter = document.querySelector("[data-profile='twitter']");
                const linkedin = document.querySelector("[data-profile='linkedin']");
                github.style.display = data.gitHubUrl ? "inline-block" : "none";
                twitter.style.display = data.twitterUrl ? "inline-block" : "none";
                linkedin.style.display = data.linkedInUrl ? "inline-block" : "none";
                github.href = data.gitHubUrl ?? "#";
                twitter.href = data.twitterUrl ?? "#";
                linkedin.href = data.linkedInUrl ?? "#";

                // Proyectos
                const carouselInner = document.querySelector("#projectCarousel .carousel-inner");
                if (carouselInner) {
                    let html = "";
                    for (let i = 0; i < data.projects.length; i += 3) {
                        const chunk = data.projects.slice(i, i + 3);
                        html += `<div class="carousel-item ${i === 0 ? 'active' : ''}"><div class="row gx-4">`;

                        for (const project of chunk) {
                            const tagsHtml = project.tags.map(tag =>
                                `<span class="badge bg-primary bg-gradient rounded-pill">${tag}</span>`).join('');
                            const desc = project.description.length > 150
                                ? project.description.slice(0, 147) + "..."
                                : project.description;

                            html += `
                                <div class="col-lg-4 mb-4">
                                    <div class="card h-100 shadow border-0">
                                        <img class="card-img-top" src="/img/Projects/${project.imageFileName}" style="object-fit: cover; height: 200px;" />
                                        <div class="card-body p-4">
                                            <div class="d-flex flex-wrap gap-1 mb-2">${tagsHtml}</div>
                                            <div class="h5 card-title mb-2">${project.title}</div>
                                            <p class="card-text text-muted">${desc}</p>
                                        </div>
                                    </div>
                                </div>
                            `;
                        }

                        html += `</div></div>`;
                    }

                    carouselInner.innerHTML = html;
                }
            })
            .catch(err => console.error("Error al cargar perfil dinámico:", err));
    };

    // Cargar el primer perfil al iniciar
    const firstItem = carousel.querySelector(".carousel-item.active");
    if (firstItem) {
        const initialId = firstItem.getAttribute("data-user-id");
        loadProfile(initialId);
    }

    // Evento al mover carrusel
    carousel.addEventListener("slid.bs.carousel", function (event) {
        const activeItem = event.relatedTarget;
        const userId = activeItem.getAttribute("data-user-id");
        loadProfile(userId);
    });
});
