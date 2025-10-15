const API_URL = "https://localhost:44322/api";

fetch(`${API_URL}/Bans`)
  .then(res => res.json())
  .then(data => console.log(data))
  .catch(err => console.error("Lỗi API:", err));

const tables = [
  {id:1, zone:'Tầng 1', status:'free'},
  {id:2, zone:'Tầng 1', status:'occupied'},
  {id:3, zone:'Tầng 2', status:'free'},
];

const grid = document.getElementById('tableGrid');
tables.forEach(t=>{
  const btn = document.createElement('button');
  btn.innerText = `Bàn ${t.id}\n(${t.status})`;
  btn.className = `table ${t.status}`;
  btn.onclick = ()=> selectTable(t);
  grid.appendChild(btn);
});
