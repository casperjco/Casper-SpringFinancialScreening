import { useState, useEffect } from 'react';
import { FixedSizeList as List } from "react-window"; // virtualized list for efficiency
import debounce from "lodash.debounce";

export default function ProductList() {
    const [query, setQuery] = useState("");
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(false);

    // Fetch products from API
    const fetchProducts = async (q = "") => {
        setLoading(true);
        try {
            const res = await fetch(`/api/products?query=${encodeURIComponent(q)}`);
            const data = await res.json();
            setProducts(data);
        } catch (err) {
            console.error("Error fetching products:", err);
        } finally {
            setLoading(false);
        }
    };

    // Initial load
    useEffect(() => {
        fetchProducts("");
    }, []);

	// Debounced search to avoid excessive API calls
    const debouncedFetch = debounce((val) => {
        fetchProducts(val);
    }, 400); // adjust debounce delay as needed

    const handleInput = (e) => {
        const val = e.target.value;
        setQuery(val);
        debouncedFetch(val);
    };

    // Row renderer for react-window
    const Row = ({ index, style }) => {
        const p = products[index];
        return (
            <div style={style} className="border-b p-2">
                <strong>{p.name}</strong> — {p.brand}
                <br />
                <span className="text-sm text-gray-600">
                    {p.category} | {p.sku}
                </span>
                <br />
                <span>${p.price.toFixed(2)} | Stock: {p.stockAmount}</span>
            </div>
        );
    };

    return (
        <div className="p-4">
            <input
                type="text"
                value={query}
                onChange={handleInput}
                placeholder="Search products..."
                className="border p-2 mb-4 w-full"
            />

            {loading && <div>Loading...</div>}

            {!loading && products.length === 0 && (
                <div className="text-gray-500">No products found.</div>
            )}

            {!loading && products.length > 0 && (
                <List
                    height={500} // container height
                    itemCount={products.length}
                    itemSize={80} // each row height in px
                    width={"100%"}
                >
                    {Row}
                </List>
            )}
        </div>
    );
}