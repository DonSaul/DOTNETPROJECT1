import { useState } from "react";
import { ToastContainer, toast } from "react-toastify";

export const Login = () => {
    const [user, setUser] = useState("");
    const [pass, setPass] = useState("");

    const handleLogin = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        fetch("https://localhost:7178/api/Login", {
            method: "POST",
            body: JSON.stringify({
                user,
                pass,
            }),
            headers: {"Content-type": "application/json"}
        }).then(res => {
            if (res.status === 401) {
                toast.error("Username and/or password is incorrect", {
                    position: "top-right",
                    autoClose: 5000,
                    pauseOnHover: true,
                    theme: "dark",
                });
            } else if (!res.ok) {
                toast.error("A server error ocurred", {
                    position: "top-right",
                    autoClose: 5000,
                    pauseOnHover: true,
                    theme: "dark",
                });
            } else {
                return res.json();
            }
            return null;
        }).then(content => {
            if (content) {
                console.log(content);
                
            }
        })
    }

    return (
        <>
        <ToastContainer/>
        <form style={{
            display: "flex",
            flexDirection: 'column',
            gap: '20px',
        }} onSubmit={handleLogin}>
            <label>Username:</label>
            <input 
                type="text" 
                name="user" 
                value={user} 
                onChange={e => setUser(e.target.value)}
            />
            <label>Password:</label>
            <input 
            type="password"
            name="pass" 
            value={pass} 
            onChange={e => setPass(e.target.value)}
            />
            <button type="submit">Login</button>
        </form>
        </>
    );
}