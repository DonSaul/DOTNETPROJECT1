import AsyncStorage from '@react-native-async-storage/async-storage';
export const apiGet = async (url: string) => {
    const token = await AsyncStorage.getItem('token');
    const headers = new Headers();
    if (token) {
        headers.set('Authorization', `Bearer ${token}`)
    }
    return fetch(url, { headers });
}