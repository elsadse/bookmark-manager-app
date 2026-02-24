export function CheckIcon({ className, onClick }: { className?: string, onClick?:()=>void }) {

    return (
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20"
            className={`fill-neutral-0 stroke-neutral-900 ${className}`} onClick={onClick}>
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.6"
                d="M16.666 5 7.5 14.167 3.333 10" />
        </svg>
    )
}