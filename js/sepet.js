// Sepet verilerini saklamak için bir dizi
let sepet = [];

// Sepet güncelleme fonksiyonu
function sepetiGuncelle() {
    const sepetUrunler = document.querySelector(".sepet-urunler");
    const toplamFiyatSpan = document.querySelector(".total-price");
    let toplamFiyat = 0;

    // Önce mevcut ürünleri temizle
    sepetUrunler.innerHTML = "";

    // Her ürün için bir sepet öğesi oluştur
    sepet.forEach((item, index) => {
        const sepetItem = document.createElement("div");
        sepetItem.classList.add("sepet-item", "flex", "justify-between", "items-center", "mb-4", "border-b", "pb-2");
        sepetItem.innerHTML = `
            <div class="item-info">
                <h4 class="font-semibold">${item.name}</h4>
                <p class="text-sm text-gray-600">${item.price} TL</p>
            </div>
            <div class="remove-container">
                <button class="remove-item bg-red-500 text-white px-2 py-1 rounded-md hover:bg-red-600" data-index="${index}">
                    Kaldır
                </button>
            </div>
        `;
        sepetUrunler.appendChild(sepetItem);
        toplamFiyat += item.price;
    });

    // Toplam fiyatı güncelle
    toplamFiyatSpan.textContent = `Toplam: ₺${toplamFiyat}`;

    // Kaldır butonlarına tıklama olayını dinle
    const removeButtons = document.querySelectorAll(".remove-item");
    removeButtons.forEach(button => {
        button.addEventListener("click", (e) => {
            const index = e.target.getAttribute("data-index");
            sepet.splice(index, 1); // Ürünü sepetten kaldır
            sepetiGuncelle(); // Sepeti yeniden oluştur
        });
    });
}

// Sepete ürün ekleme işlevi
function sepeteEkle(name, price) {
    sepet.push({ name, price });
    sepetiGuncelle();
}

// "add-to-cart" butonlarına tıklama olayını dinle
const addToCartButtons = document.querySelectorAll(".add-to-cart");
addToCartButtons.forEach(button => {
    button.addEventListener("click", (e) => {
        e.preventDefault(); // Sayfanın yenilenmesini önle
        const name = button.getAttribute("data-name");
        const price = parseFloat(button.getAttribute("data-price"));
        sepeteEkle(name, price);
    });
});

// Sepet satın alma butonuna tıklama işlevi
const sepetButton = document.querySelector(".sepet-button");
sepetButton.addEventListener("click", () => {
    const tarih = document.getElementById("tarih").value;
    const saat = document.getElementById("saat").value;
    
    // Geçerli tarihi al
    const today = new Date().toISOString().split("T")[0]; // YYYY-MM-DD formatında al

    // Seçilen tarihi kontrol et
    if (tarih < today) {
        alert("Randevu Oluşturulamamıştır ! Lütfen doğru bir tarih giriniz.");
    } else if (tarih && saat) {
        alert(`Hizmetiniz için randevu başarılı ile oluşturulmuştur.\nRandevu Tarihi: ${tarih} Saat: ${saat}`);
    } else {
        alert("Lütfen tarih ve saat seçiniz.");
    }
});


