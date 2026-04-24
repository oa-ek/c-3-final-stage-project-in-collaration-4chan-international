export interface InputContentItemDTO {
    name: string;
    route: string;
    // Зверни увагу: саме зображення (IFormFile) ми будемо передавати
    // через FormData, тому в TypeScript інтерфейсі воно не обов'язкове,
    // але цей DTO допоможе нам пам'ятати про потрібні текстові поля.
}
