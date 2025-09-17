window.emojiMartInterop = {
    init: function (buttonId, pickerId, inputSelector) {
        const button = document.getElementById(buttonId);
        const pickerContainer = document.getElementById(pickerId);
        let pickerVisible = false;

        if (!button || !pickerContainer) return;

        const input = document.querySelector(inputSelector);

        if (!input) return;

        let lastSelectionStart = 0;
        let lastSelectionEnd = 0;

        input.addEventListener("click", () => {
            lastSelectionStart = input.selectionStart;
            lastSelectionEnd = input.selectionEnd;
        });

        const picker = new EmojiMart.Picker({
            set: "native",
            onEmojiSelect: (emoji) => {
                const value = input.value;

                const start = lastSelectionStart ?? input.selectionStart ?? 0;
                const end = lastSelectionEnd ?? input.selectionEnd ?? 0;

                input.value =
                    value.substring(0, start) +
                    emoji.native +
                    value.substring(end);

                const newPos = start + emoji.native.length;
                input.selectionStart = input.selectionEnd = newPos;

                lastSelectionStart = lastSelectionEnd = newPos;

                input.dispatchEvent(new Event("change", { bubbles: true }));
                input.focus();
            }
        });

        pickerContainer.appendChild(picker);
        pickerContainer.style.display = "none";

        button.addEventListener("click", () => {
            pickerVisible = !pickerVisible;
            pickerContainer.style.display = pickerVisible ? "block" : "none";

            if (pickerVisible) {
                input.focus();
            }
        });
    }
};
