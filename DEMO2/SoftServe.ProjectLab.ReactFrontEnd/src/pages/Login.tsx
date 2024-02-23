import { useState } from "react";

export const Login = () => {
    const [user, setUser] = useState("");
    const [pass, setPass] = useState("");
    return (
        <form onSubmit={() => {}}>
            <label>Username:
                <input 
                    type="text" 
                    name="user" 
                    value={user} 
                    onChange={e => setUser(e.target.value)}
                />
            </label>
            <label>Password:
                <input 
                type="password"
                name="pass" 
                value={pass} 
                onChange={e => setPass(e.target.value)}
                />
            </label>
            <button type="submit">Login</button>
        </form>
    );
}