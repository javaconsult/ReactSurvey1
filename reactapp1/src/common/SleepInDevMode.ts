export default async function SleepInDevMode(){
    const sleep = (ms: number) => new Promise(resolve => setTimeout(resolve, ms));
    process.env.NODE_ENV === 'development' && await sleep(2000); //artificially pause when in development
}