// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const teamMembers = ['Co-workers', 'Friends', 'Family', 'Just you', 'Students', 'Volunteers'];
const carouselElement = document.getElementById('text-carousel');
let index = 0;

function showNextMember() {
    if (carouselElement != null) {
        const currentWord = teamMembers[index];
        const coloredWord = `<span class="word${index + 1}">${currentWord}</span>`;
        carouselElement.innerHTML = coloredWord;

        index = (index + 1) % teamMembers.length;
        setTimeout(showNextMember, 2000); // Change the time here (in milliseconds)
    }
}

showNextMember();

