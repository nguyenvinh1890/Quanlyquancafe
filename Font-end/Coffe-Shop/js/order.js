const API_URL = "https://localhost:44322/api";

fetch(`${API_URL}/Bans`)
  .then(res => res.json())
  .then(data => console.log(data))
  .catch(err => console.error("Lỗi API:", err));

let currentTable=null;
function selectTable(t){
  currentTable=t;
  alert(`Chọn ${t.zone} - Bàn ${t.id}`);
}

// demo menu
const menu = [
  {id:1, name:'Cà phê sữa', price:30000},
  {id:2, name:'Trà đào cam sả', price:35000}
];

const menuGrid = document.getElementById('menuGrid');
menu.forEach(m=>{
  const btn = document.createElement('button');
  btn.innerText = `${m.name}\n${m.price}đ`;
  btn.onclick=()=> addToOrder(m);
  menuGrid.appendChild(btn);
});

let order=[];
function addToOrder(item){
  order.push(item);
  renderOrder();
}

function renderOrder(){
  const list = document.getElementById('orderList');
  list.innerHTML = order.map(o=>`<div>${o.name} - ${o.price}đ</div>`).join('');
}

document.getElementById('sendKitchenBtn').onclick=()=>{
  if(!currentTable || order.length===0) return alert('Chọn bàn và món trước!');
  alert('Đã gửi order xuống bếp!');
  order=[];
  renderOrder();
};
