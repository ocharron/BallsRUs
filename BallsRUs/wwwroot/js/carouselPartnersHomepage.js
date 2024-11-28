const cardWrapper = document.querySelector('.homepage-carousel-partners-cardwrapper')
const cardWrapperChildren = Array.from(cardWrapper.children)
const widthToScroll = cardWrapper.children[0].offsetWidth
const arrowPrev = document.querySelector('.arrow.prev')
const arrowNext = document.querySelector('.arrow.next')
const cardBounding = cardWrapper.getBoundingClientRect()
const column = Math.floor(cardWrapper.offsetWidth / (widthToScroll + 24))
let currScroll = 0
let initPos = 0
let clicked = false

cardWrapper.classList.add('no-smooth')
cardWrapper.scrollLeft = cardWrapper.offsetWidth
cardWrapper.classList.remove('no-smooth')

cardWrapperChildren.slice(-column).reverse().forEach(item => {
    cardWrapper.insertAdjacentHTML('afterbegin', item.outerHTML)
})

cardWrapperChildren.slice(0, column).forEach(item => {
    cardWrapper.insertAdjacentHTML('beforeend', item.outerHTML)
})

const cardImageAndLink = cardWrapper.querySelectorAll('img, a')
cardImageAndLink.forEach(item => {
    item.setAttribute('draggable', false)
})

arrowPrev.onclick = function () {
    cardWrapper.scrollLeft -= widthToScroll
}

arrowNext.onclick = function () {
    cardWrapper.scrollLeft += widthToScroll
}

cardWrapper.onmousedown = function (e) {
    cardWrapper.classList.add('grab')
    initPos = e.clientX - cardBounding.left
    currScroll = cardWrapper.scrollLeft
    clicked = true
}

cardWrapper.onmousemove = function (e) {
    if (clicked) {
        const xPos = e.clientX - cardBounding.left
        cardWrapper.scrollLeft = currScroll + -(xPos - initPos)
    }
}

cardWrapper.onmouseup = mouseUpAndLeave
cardWrapper.onmouseleave = mouseUpAndLeave

function mouseUpAndLeave() {
    cardWrapper.classList.remove('grab')
    clicked = false
}

let autoScroll

cardWrapper.onscroll = function () {
    if (cardWrapper.scrollLeft === 0) {
        // Réinsérez les éléments à la fin lorsque vous atteignez le début
        const lastChildren = cardWrapperChildren.slice();
        lastChildren.reverse().forEach(item => {
            cardWrapper.insertAdjacentHTML('afterbegin', item.outerHTML);
        });
    } else if (cardWrapper.scrollLeft >= cardWrapper.scrollWidth - (cardWrapper.offsetWidth + 24)) {
        // Réinsérez les éléments au début lorsque vous atteignez la fin
        const firstChildren = cardWrapperChildren.slice();
        firstChildren.reverse().forEach(item => {
            cardWrapper.insertAdjacentHTML('beforeend', item.outerHTML);
        });
    }

    if (autoScroll) {
        clearTimeout(autoScroll);
    }

    autoScroll = setTimeout(() => {
        cardWrapper.classList.remove('no-smooth');
        cardWrapper.scrollLeft += widthToScroll;
    }, 5000);
}