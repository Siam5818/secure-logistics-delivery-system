import { useState } from 'react';

const GATEWAY_URL = 'http://localhost:5007';

type OrderLine = {
  id: string;
  productName: string;
  quantity: number;
  unitPrice: { amount: number; currency: string };
};

type Order = {
  id: string;
  status: number;
  shippingAddress: { street: string; city: string; postalCode: string; country: string };
  lines: OrderLine[];
  total: { amount: number; currency: string };
};

const STATUS_LABELS = ['Created', 'Paid', 'Shipped', 'Delivered', 'Cancelled'];

function App() {
  const [token, setToken] = useState<string | null>(null);
  const [order, setOrder] = useState<Order | null>(null);
  const [error, setError] = useState<string | null>(null);

  const [street, setStreet] = useState('1 Rue de la Paix');
  const [city, setCity] = useState('Dakar');
  const [postalCode, setPostalCode] = useState('10000');
  const [country, setCountry] = useState('Senegal');
  const [currency, setCurrency] = useState('XOF');

  const [productName, setProductName] = useState('Laptop');
  const [quantity, setQuantity] = useState(1);
  const [unitPrice, setUnitPrice] = useState(500000);

  async function ensureToken(): Promise<string> {
    if (token) return token;
    const res = await fetch(`${GATEWAY_URL}/api/auth/token`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ username: 'testuser' }),
    });
    const data = await res.json();
    setToken(data.access_token);
    return data.access_token;
  }

  async function fetchOrder(id: string, authToken: string) {
    const res = await fetch(`${GATEWAY_URL}/api/orders/${id}`, {
      headers: { Authorization: `Bearer ${authToken}` },
    });
    const data = await res.json();
    setOrder(data);
  }

  async function handleCreateOrder() {
    setError(null);
    try {
      const authToken = await ensureToken();
      const res = await fetch(`${GATEWAY_URL}/api/orders`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${authToken}`,
        },
        body: JSON.stringify({ street, city, postalCode, country, currency }),
      });
      if (!res.ok) throw new Error(`Create failed: ${res.status}`);
      const data = await res.json();
      await fetchOrder(data.id, authToken);
    } catch (e) {
      setError((e as Error).message);
    }
  }

  async function handleAddLine() {
    if (!order || !token) return;
    setError(null);
    try {
      const res = await fetch(`${GATEWAY_URL}/api/orders/${order.id}/lines`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({ productName, quantity, unitPrice, currency }),
      });
      if (!res.ok) throw new Error(`Add line failed: ${res.status}`);
      await fetchOrder(order.id, token);
    } catch (e) {
      setError((e as Error).message);
    }
  }

  async function handlePay() {
    if (!order || !token) return;
    setError(null);
    try {
      const res = await fetch(`${GATEWAY_URL}/api/orders/${order.id}/pay`, {
        method: 'POST',
        headers: { Authorization: `Bearer ${token}` },
      });
      if (!res.ok) throw new Error(`Pay failed: ${res.status}`);
      await fetchOrder(order.id, token);
    } catch (e) {
      setError((e as Error).message);
    }
  }

  return (
    <div className="app-container">
      <h1>Secure Logistics & Delivery System</h1>

      {error && <p className="error">{error}</p>}

      <div className="layout">
        <section>
          <h2>1. Create Order</h2>
          <input value={street} onChange={(e) => setStreet(e.target.value)} placeholder="Street" />
          <input value={city} onChange={(e) => setCity(e.target.value)} placeholder="City" />
          <input value={postalCode} onChange={(e) => setPostalCode(e.target.value)} placeholder="Postal Code" />
          <input value={country} onChange={(e) => setCountry(e.target.value)} placeholder="Country" />
          <input value={currency} onChange={(e) => setCurrency(e.target.value)} placeholder="Currency" />
          <button onClick={handleCreateOrder}>Create Order</button>
        </section>

        <section>
          {order ? (
            <>
              <h2>2. Order {order.id.slice(0, 8)}…</h2>
              <p>
                Status:{' '}
                <strong className={`status-${STATUS_LABELS[order.status].toLowerCase()}`}>
                  {STATUS_LABELS[order.status]}
                </strong>
              </p>
              <p>Total: {order.total.amount} {order.total.currency}</p>

              <h3>Lines</h3>
              <ul>
                {order.lines.map((line) => (
                  <li key={line.id}>
                    {line.productName} x{line.quantity} — {line.unitPrice.amount} {line.unitPrice.currency}
                  </li>
                ))}
              </ul>

              {STATUS_LABELS[order.status] === 'Created' && (
                <>
                  <h3>Add Line</h3>
                  <input value={productName} onChange={(e) => setProductName(e.target.value)} placeholder="Product" />
                  <input type="number" value={quantity} onChange={(e) => setQuantity(Number(e.target.value))} />
                  <input type="number" value={unitPrice} onChange={(e) => setUnitPrice(Number(e.target.value))} />
                  <button onClick={handleAddLine}>Add Line</button>

                  <br /><br />
                  <button onClick={handlePay} disabled={order.lines.length === 0}>Pay Order</button>
                </>
              )}
            </>
          ) : (
            <p className="empty-state">No order yet — create one on the left.</p>
          )}
        </section>
      </div>
    </div>
  );
}

export default App;
