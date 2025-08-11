import './main.scss';
import { getDoors, getUsers, getLogs } from './api';

async function init() {
  const [doors, users, logs] = await Promise.all([
    getDoors(),
    getUsers(),
    getLogs()
  ]);
  console.log('Doors', doors);
  console.log('Users', users);
  console.log('Logs', logs);
}

init();
