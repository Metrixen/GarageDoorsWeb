export async function getDoors() {
  const response = await fetch('/api/door');
  return response.json();
}

export async function getUsers() {
  const response = await fetch('/api/user');
  return response.json();
}

export async function getLogs() {
  const response = await fetch('/api/log');
  return response.json();
}
