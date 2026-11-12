const form = document.getElementById('bbq-form');
const success = document.getElementById('success');
const password = document.getElementById('password');
let submitted = false;

function validateField(input) {
  let message = '';
  if (!input.value.trim()) {
    const messages = {
      name: 'Vul je naam in.',
      email: 'Vul je e-mailadres in.',
      password: 'Vul een wachtwoord in.',
      food: 'Vul in welk vlees je meeneemt.',
      drink: 'Vul in welke drank je meeneemt.'
    };
    message = messages[input.name];
  } else if (input.type === 'email' && input.validity.typeMismatch) {
    message = 'Vul een geldig e-mailadres in, zoals jij@voorbeeld.be.';
  } else if (input.name === 'password' && input.value.length < 8) {
    message = 'Je wachtwoord moet minstens 8 tekens bevatten.';
  }
  document.getElementById(`${input.id}-error`).textContent = message;
  input.setAttribute('aria-invalid', String(Boolean(message)));
  return !message;
}

const fields = ['name', 'email', 'password', 'food', 'drink'].map(id => document.getElementById(id));
fields.forEach(input => {
  input.addEventListener('blur', () => validateField(input));
  input.addEventListener('input', () => {
    success.hidden = true;
    if (submitted || input.getAttribute('aria-invalid') === 'true') validateField(input);
  });
});

document.getElementById('toggle-password').addEventListener('click', event => {
  const show = password.type === 'password';
  password.type = show ? 'text' : 'password';
  event.currentTarget.textContent = show ? 'Verberg' : 'Toon';
  event.currentTarget.setAttribute('aria-pressed', String(show));
});

form.addEventListener('submit', event => {
  event.preventDefault();
  submitted = true;
  const validFields = fields.map(validateField).every(Boolean);
  success.hidden = !validFields;
  if (!validFields) form.querySelector('[aria-invalid="true"]').focus();
});
